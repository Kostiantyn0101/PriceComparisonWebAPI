using BLL.Services.MediaServices;
using BLL.Services.ProductServices;
using DLL.Repository;
using Domain.Models.Configuration;
using Domain.Models.DBModels;
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

        public async Task<OperationResultModel<string>> ProcessXmlAsync(Stream stream)
        {
            var xmlDoc = await XDocument.LoadAsync(stream, LoadOptions.None, CancellationToken.None);
            var priceListDate = DateTime.Parse(xmlDoc.Root.Attribute("date").Value);
            var apiKey = xmlDoc.Root.Element("api_key").Value;

            // Seller search
            var seller = (await _sellerRepository.GetFromConditionAsync(s => s.ApiKey == apiKey))
                .FirstOrDefault();

            if (seller == null)
            {
                return OperationResultModel<string>.Failure($"Seller with api key {apiKey} not found");
            }

            if (!seller.IsActive || seller.AccountBalance < _accountConfiguration.MinBalanceToProceed)
            {
                return OperationResultModel<string>.Failure($"Seller with api key {apiKey} is inactive or has no account balance");
            }

            // Currency processing
            Dictionary<string, decimal> currencies = xmlDoc.Root?.Element("currencies") is XElement currenciesElement
                && currenciesElement.Elements("currency").Any()
                ? currenciesElement.Elements("currency")
                    .ToDictionary(
                        c => c.Attribute("id").Value,
                        c => decimal.Parse(c.Attribute("rate").Value)
                    )
                : new Dictionary<string, decimal>();

            // Categories processing
            Dictionary<string, string> categories = xmlDoc.Root?.Element("categories") is XElement categoriesElement
                && categoriesElement.Elements("category").Any()
                ? categoriesElement.Elements("category")
                   .ToDictionary(
                        c => c.Attribute("id")?.Value.Trim() ?? string.Empty,  // Key: ID
                        c => c.Value.Trim()  // Value: Category name
                    )
                : new Dictionary<string, string>();

            // Product processing
            foreach (var offer in xmlDoc.Root.Element("offers").Elements("offer"))
            {
                var gtin = offer.Element("gtin")?.Value ?? string.Empty;
                var title = offer.Element("name")?.Value ?? string.Empty;
                var normalizedTitle = title.Trim().ToUpperInvariant();

                var product = await _productRepository.GetQuery()
                    .Where(p => (!string.IsNullOrWhiteSpace(gtin) && (p.GTIN == gtin || p.UPC == gtin)) ||
                                (string.IsNullOrWhiteSpace(gtin) && p.BaseProduct.NormalizedTitle.Equals(normalizedTitle)))
                    .FirstOrDefaultAsync();
                

                // Create new product if not found
                if (product == null)
                {
                    //var categoryNormalizedTitle = offer.Element("category")?.Value.Trim().ToUpperInvariant() ?? string.Empty;
                    var categoryFileId = offer.Element("categoryId")?.Value.Trim() ?? string.Empty;
                    var categoryTitle = categories.ContainsKey(categoryFileId) ? categories[categoryFileId] : string.Empty;
                    var category = (await _categoryRepository.GetFromConditionAsync(c => c.Title.Equals(categoryTitle))).FirstOrDefault();
                    if (category == null)
                    {
                        category = (await _categoryRepository.GetFromConditionAsync(c => c.Title.Equals(defaultCategoryName))).FirstOrDefault();
                    }

                    var newBaseProduct = new BaseProductDBModel
                    {
                        Title = title,
                        Brand = offer.Element("brand")?.Value ?? string.Empty,
                        Description = offer.Element("description")?.Value ?? string.Empty,
                        CategoryId = category?.Id ?? 0,
                        IsUnderModeration = true,
                        AddedToDatabase = DateTime.UtcNow,
                    };

                    product = new ProductDBModel
                    {
                        GTIN = gtin,
                        ModelNumber = offer.Element("model")?.Value ?? string.Empty,
                        IsUnderModeration = true,
                        AddedToDatabase = DateTime.UtcNow,
                        BaseProduct = newBaseProduct,
                    };

                    var createResult = await _productRepository.CreateAsync(product);
                    if (!createResult.IsSuccess)
                    {
                        _logger.LogError(createResult.Exception, "New product create error");
                        continue;
                    }
                    var createdProduct = createResult.Data;

                    // Save product image
                    var imageUrl = offer.Element("picture")?.Value;
                    if (!string.IsNullOrEmpty(imageUrl))
                    {
                        // to do: fix file ctreation error

                        //var imageFile = await _fileService.CreateFormFileFromUrlAsync(imageUrl);
                        //var imageAddResult = await _productImageService.AddAsync(new ProductImageCreateRequestModel()
                        //{
                        //    ProductId = createdProduct?.Id ?? 0,
                        //    Images = new List<Microsoft.AspNetCore.Http.IFormFile> { imageFile }
                        //});
                    }
                    continue; // ??
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

            return OperationResultModel<string>.Success();
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
            var query = BuildSellerProductDetailsQuery(spd => spd.Product.BaseProductId == model.BaseProductId && spd.Product.ProductGroupId==model.ProductGroupId);
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
