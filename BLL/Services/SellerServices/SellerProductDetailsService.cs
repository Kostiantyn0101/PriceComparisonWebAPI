using BLL.Services.MediaServices;
using BLL.Services.ProductServices;
using DLL.Repository;
using Domain.Models.Configuration;
using Domain.Models.DBModels;
using Domain.Models.Request.Products;
using Domain.Models.Request.Seller;
using Domain.Models.Response;
using Domain.Models.Response.Seller;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Linq.Expressions;
using System.Xml.Linq;

namespace BLL.Services.SellerServices

{
    public class SellerProductDetailsService : ISellerProductDetailsService
    {
        private readonly ICompositeKeyRepository<SellerProductDetailsDBModel, int, int> _repository;
        private readonly IRepository<ProductDBModel, int> _productRepository;
        private readonly IRepository<SellerDBModel, int> _sellerRepository;
        private readonly IRepository<CategoryDBModel, int> _categoryRepository;
        private readonly IRepository<PriceHistoryDBModel, int> _priceHistoryRepository;
        private readonly SellerAccountConfiguration _accountConfiguration;
        private readonly FileStorageConfiguration _fileStorageConfiguration;
        private readonly IProductImageService _productImageService;
        private readonly IFileService _fileService;
        private readonly ILogger<SellerProductDetailsService> _logger;

        private const string defaultCategoryName = "NEW PRODUCT UNKNOWN CATEGORY";
        private const int defaultCategoryId = 163;  // from the database
        private const int defaultProductGroupId = 21;  // from the database

        public SellerProductDetailsService(ICompositeKeyRepository<SellerProductDetailsDBModel, int, int> repository,
            IRepository<ProductDBModel, int> productRepository,
            IRepository<SellerDBModel, int> sellerRepository,
            IRepository<CategoryDBModel, int> categoryRepository,
            IRepository<PriceHistoryDBModel, int> priceHistoryRepository,
            IProductImageService productImageService,
            IFileService fileService,
            IOptions<SellerAccountConfiguration> options,
            IOptions<FileStorageConfiguration> fileOptions,
            ILogger<SellerProductDetailsService> logger)
        {
            _repository = repository;
            _productRepository = productRepository;
            _sellerRepository = sellerRepository;
            _categoryRepository = categoryRepository;
            _priceHistoryRepository = priceHistoryRepository;
            _accountConfiguration = options.Value;
            _productImageService = productImageService;
            _fileService = fileService;
            _logger = logger;
            _fileStorageConfiguration = fileOptions.Value;
        }

        public async Task<OperationResultModel<IEnumerable<string>>> ProcessXmlAsync(Stream stream)
        {
            var uploadResults = new List<string>();

            var xmlDoc = await XDocument.LoadAsync(stream, LoadOptions.None, CancellationToken.None);

            DateTime priceListDate;
            if (!DateTime.TryParse(xmlDoc?.Root?.Attribute("date")?.Value ?? string.Empty, out priceListDate))
            {
                return OperationResultModel<IEnumerable<string>>.Failure($"Upload Error. Price list date is wrong or absent");
            }

            var apiKey = xmlDoc?.Root?.Element("api_key")?.Value;
            if (apiKey == null)
            {
                return OperationResultModel<IEnumerable<string>>.Failure($"Upload Error. Api key is invalid or absent");
            }

            var seller = (await _sellerRepository.GetFromConditionAsync(s => s.ApiKey == apiKey)).FirstOrDefault();

            if (seller == null)
            {
                return OperationResultModel<IEnumerable<string>>.Failure($"Upload Error. Seller with api key {apiKey} not found");
            }

            if (!seller.IsActive || seller.AccountBalance < _accountConfiguration.MinBalanceToProceed)
            {
                return OperationResultModel<IEnumerable<string>>.Failure($"Upload Error. Seller with api key {apiKey} is inactive or has no account balance");
            }

            // Currency processing
            Dictionary<string, decimal> currencies = new Dictionary<string, decimal>();
            if (xmlDoc?.Root?.Element("currencies") is XElement currenciesXElement && currenciesXElement.Elements("currency").Any())
            {
                var currenciyElements = currenciesXElement.Elements("currency").ToList();

                foreach (var currency in currenciyElements)
                {
                    var idAttribute = currency.Attribute("id");
                    if (idAttribute == null || string.IsNullOrWhiteSpace(idAttribute.Value))
                        return OperationResultModel<IEnumerable<string>>.Failure("Error: A 'currency' element is missing the 'id' attribute or its value is empty.");

                    var currencyId = idAttribute.Value;
                    if (currencies.ContainsKey(currencyId))
                        return OperationResultModel<IEnumerable<string>>.Failure($"Error: Duplicate currency id '{currencyId}' found.");

                    var rateAttribute = currency.Attribute("rate");
                    if (rateAttribute == null)
                        return OperationResultModel<IEnumerable<string>>.Failure($"Error: Currency with id '{idAttribute.Value}' is missing the 'rate' attribute.");

                    if (!decimal.TryParse(rateAttribute.Value, out decimal rate))
                        return OperationResultModel<IEnumerable<string>>.Failure($"Error: Invalid 'rate' value for currency with id '{idAttribute.Value}': {rateAttribute.Value}");

                    currencies.Add(idAttribute.Value, rate);
                }
            };


            // Categories processing
            Dictionary<string, string> categories = new Dictionary<string, string>();
            if (xmlDoc?.Root?.Element("categories") is XElement categoriesXElement && categoriesXElement.Elements("category").Any())
            {
                var categoryElements = categoriesXElement.Elements("category").ToList();

                foreach (var category in categoryElements)
                {
                    var idAttribute = category.Attribute("id");
                    if (idAttribute == null || string.IsNullOrWhiteSpace(idAttribute.Value))
                        return OperationResultModel<IEnumerable<string>>.Failure("Error: A 'category' element is missing the 'id' attribute or its value is empty.");

                    string categoryId = idAttribute.Value.Trim();
                    if (categories.ContainsKey(categoryId))
                        return OperationResultModel<IEnumerable<string>>.Failure($"Error: Duplicate category id '{categoryId}' found.");

                    string categoryName = category.Value.Trim();
                    if (string.IsNullOrWhiteSpace(categoryName))
                        return OperationResultModel<IEnumerable<string>>.Failure($"Error: Category with id '{idAttribute.Value}' has an empty name.");

                    categories.Add(idAttribute.Value.Trim(), categoryName);
                }
            }

            var offers = xmlDoc?.Root?.Element("offers")?.Elements("offer");
            if (offers.IsNullOrEmpty())
                return OperationResultModel<IEnumerable<string>>.Failure($"Error: No product offers found.");

            // Product processing
            foreach (var offer in offers!)
            {
                var gtin = offer.Element("gtin")?.Value ?? string.Empty;
                var modelNumber = offer.Element("model")?.Value ?? string.Empty;
                var title = offer.Element("name")?.Value ?? string.Empty;

                if (string.IsNullOrEmpty(gtin) && string.IsNullOrEmpty(modelNumber))
                {
                    uploadResults.Add($"Error. Product with name {title} has neither GTIN code nor manufacturer model. Price wasn't set");
                    continue;
                }

                var normalizedTitle = title.Trim().ToUpperInvariant();
                var normalizedModelNumber = modelNumber.Trim().ToUpperInvariant();

                var product = await _productRepository.GetQuery()
                    .Where(p => (!string.IsNullOrEmpty(gtin) && (p.GTIN == gtin || p.UPC == gtin))
                    || (!string.IsNullOrEmpty(p.NormalizedModelNumber) && p.NormalizedModelNumber == normalizedModelNumber))
                    .FirstOrDefaultAsync();

                // Create new product if not found
                if (product == null)
                {
                    uploadResults.Add($"Error. Product with GTIN code '{gtin}' and model number '{modelNumber}' wasn't found in database. Price wasn't set");

                    var categoryFileId = offer.Element("categoryId")?.Value.Trim() ?? string.Empty;
                    var categoryTitle = categories.ContainsKey(categoryFileId) ? categories[categoryFileId] : string.Empty;
                    var category = (await _categoryRepository.GetFromConditionAsync(c => c.Title.Equals(categoryTitle))).FirstOrDefault();
                    if (category == null)
                    {
                        category = (await _categoryRepository.GetFromConditionAsync(c => c.Title.Equals(defaultCategoryName))).FirstOrDefault();
                        uploadResults.Add($"Category for product with GTIN code '{gtin}' and model number '{modelNumber}' wasn't found in database. New product creation is getting more complex.");
                    }

                    var brand = offer.Element("brand")?.Value ?? string.Empty;
                    if (brand.IsNullOrEmpty())
                        uploadResults.Add($"No Brand for product with GTIN code '{gtin}' and model number '{modelNumber}' is provided. New product creation is getting more complex.");

                    var description = offer.Element("description")?.Value;
                    if (brand.IsNullOrEmpty())
                        uploadResults.Add($"No Description for product with GTIN code '{gtin}' and model number '{modelNumber}' is provided. New product creation is getting more complex.");

                    var newBaseProduct = new BaseProductDBModel
                    {
                        Title = title,
                        Brand = brand,
                        Description = description,
                        CategoryId = category?.Id ?? defaultCategoryId,
                        IsUnderModeration = true,
                        AddedToDatabase = DateTime.UtcNow,
                    };

                    product = new ProductDBModel
                    {
                        GTIN = gtin,
                        ModelNumber = modelNumber,
                        IsUnderModeration = true,
                        AddedToDatabase = DateTime.UtcNow,
                        BaseProduct = newBaseProduct,
                        ProductGroupId = defaultProductGroupId
                    };

                    var createResult = await _productRepository.CreateAsync(product);
                    if (!createResult.IsSuccess || createResult.Data == null)
                    {
                        _logger.LogError(createResult.Exception, "New product create error");
                        continue;
                    }
                    var createdProduct = createResult.Data;

                    // Save product image
                    var imageUrl = offer.Element("picture")?.Value;
                    if (!string.IsNullOrEmpty(imageUrl))
                    {
                        var imageFile = await _fileService.CreateFormFileFromUrlAsync(imageUrl);
                        var imageAddResult = await _productImageService.AddAsync(new ProductImageCreateRequestModel()
                        {
                            ProductId = createdProduct!.Id,
                            Images = new List<Microsoft.AspNetCore.Http.IFormFile> { imageFile }
                        });
                    }
                    else
                    {
                        uploadResults.Add($"No image for product with GTIN code '{gtin}' and model number '{modelNumber}' is provided. New product creation is getting more complex.");
                    }
                }

                // Price conversion to UAH
                var currencyId = offer.Element("currencyId")?.Value ?? string.Empty;
                var priceStr = offer.Element("price")?.Value;
                if (priceStr.IsNullOrEmpty()) continue;
                var price = decimal.Parse(priceStr ?? "0");
                if (price == 0) continue;
                var priceUah = currencyId == "UAH" ? price : price * currencies[currencyId];

                var productStoreUrl = offer.Element("url")?.Value;
                if (productStoreUrl.IsNullOrEmpty()) continue;

                // Write data to SellerProductDetails
                var details = (await _repository.GetFromConditionAsync(
                    d => d.ProductId == product.Id && d.SellerId == seller.Id))
                    .FirstOrDefault();

                if (product.IsUnderModeration)
                {
                    uploadResults.Add($"Error. Product with GTIN code '{gtin}' and model number '{modelNumber}' is under moderation. Price was set but won't be shown unless moderation is over");
                }

                if (details == null)
                {
                    _ = await _repository.CreateAsync(new SellerProductDetailsDBModel
                    {
                        ProductId = product.Id,
                        SellerId = seller.Id,
                        PriceValue = priceUah,
                        LastUpdated = priceListDate,
                        ProductStoreUrl = productStoreUrl!
                    });
                }
                else
                {
                    details.PriceValue = priceUah;
                    details.LastUpdated = priceListDate;
                    details.ProductStoreUrl = productStoreUrl!;
                    _ = await _repository.UpdateAsync(details);
                }

                // Write data to PriceHistory
                _ = await _priceHistoryRepository.CreateAsync(new PriceHistoryDBModel()
                {
                    ProductId = product.Id,
                    SellerId = seller.Id,
                    CreatedAt = DateTime.Now,
                    PriceDate = priceListDate,
                    PriceValue = priceUah
                });
            }

            return OperationResultModel<IEnumerable<string>>.Success();
        }


        public async Task<IEnumerable<SellerProductDetailsResponseModel>> GetSellerProductDetailsAsync(int productId)
        {
            var query = BuildSellerProductDetailsQuery(spd => spd.ProductId == productId);
            var productDetails = await query.OrderByDescending(x => x.StoreUrlClickRate).ToListAsync();
            ProcessSellerLogoImageUrl(productDetails);
            return productDetails;
        }

        public async Task<OperationResultModel<PaginatedResponse<SellerProductDetailsResponseModel>>> GetPaginatedSellerProductDetailsAsync(
            Expression<Func<SellerProductDetailsDBModel, bool>> condition, int page, int pageSize)
        {
            var query = BuildSellerProductDetailsQuery(condition);

            var totalItems = await query.CountAsync();

            var data = await query
                .OrderByDescending(x => x.StoreUrlClickRate)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            ProcessSellerLogoImageUrl(data);

            var response = new PaginatedResponse<SellerProductDetailsResponseModel>
            {
                Data = data,
                Page = page,
                PageSize = pageSize,
                TotalItems = totalItems
            };

            return OperationResultModel<PaginatedResponse<SellerProductDetailsResponseModel>>.Success(response);
        }


        public async Task<IEnumerable<SellerProductDetailsResponseModel>> GetSellerProductDetailsByProductGroupAsync(SellerProductDetailsRequestModel model)
        {
            var query = BuildSellerProductDetailsQuery(spd => spd.Product.BaseProductId == model.BaseProductId && spd.Product.ProductGroupId == model.ProductGroupId);
            var productDetails = await query.OrderByDescending(x => x.StoreUrlClickRate).ToListAsync();
            ProcessSellerLogoImageUrl(productDetails);
            return productDetails;
        }


        private IQueryable<SellerProductDetailsResponseModel> BuildSellerProductDetailsQuery(Expression<Func<SellerProductDetailsDBModel, bool>> condition)
        {
            return _repository.GetQuery()
                .Where(condition)
                .Where(spd => spd.Seller.IsActive && spd.Seller.AccountBalance > 0)
                .Select(spd => new SellerProductDetailsResponseModel
                {
                    SellerId = spd.SellerId,
                    StoreName = spd.Seller.StoreName,
                    LogoImageUrl = spd.Seller.LogoImageUrl,
                    PriceValue = spd.PriceValue,
                    ProductStoreUrl = spd.ProductStoreUrl,
                    StoreUrlClickRate = spd.Seller.AuctionClickRates
                        .Where(acr => acr.CategoryId == spd.Product.BaseProduct.CategoryId)
                        .Select(acr => (decimal?)acr.ClickRate)
                        .FirstOrDefault() ?? _accountConfiguration.DefaultClickRate
                });
        }


        public async Task<OperationResultModel<SellerProductPricesResponseModel>> GetSellerProductPricesAsync(int productId)
        {
            var query = _repository.GetQuery()
                .Where(sd => sd.ProductId == productId)
                .GroupBy(sd => sd.ProductId)
                .Select(g => new SellerProductPricesResponseModel
                {
                    ProductId = g.Key,
                    MaxPriceValue = g.Max(sd => sd.PriceValue),
                    MinPriceValue = g.Min(sd => sd.PriceValue),
                });

            var result = await query.FirstOrDefaultAsync();

            return result != null
                ? OperationResultModel<SellerProductPricesResponseModel>.Success(result)
                : OperationResultModel<SellerProductPricesResponseModel>.Failure("Prices not found error");

        }

        private void ProcessSellerLogoImageUrl(IEnumerable<SellerProductDetailsResponseModel> model)
        {
            foreach (var item in model)
            {
                item.LogoImageUrl = $"{_fileStorageConfiguration.ServerURL.TrimEnd('/')}/{item.LogoImageUrl.TrimStart('/')}";
            }
        }
    }
}
