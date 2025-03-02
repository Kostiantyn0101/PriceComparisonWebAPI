using Domain.Models.DBModels;
using Microsoft.Extensions.Options;
using System.Xml.Linq;
using DLL.Repository;
using Domain.Models.Configuration;
using Domain.Models.Response;
using BLL.Services.ProductServices;
using BLL.Services.MediaServices;
using Domain.Models.Request.Products;
using Microsoft.Extensions.Logging;
using Domain.Models.Exceptions;

namespace BLL.Services.SellerServices
{
    public class SellerProductService : ISellerProductService
    {
        private readonly ISellerProductDetailsRepository _repository;
        private readonly IRepository<ProductDBModel> _productRepository;
        private readonly IRepository<SellerDBModel> _sellerRepository;
        private readonly IRepository<CategoryDBModel> _categoryRepository;
        private readonly IRepository<PriceHistoryDBModel> _priceHistoryRepository;
        private readonly SellerAccountConfiguration _accountConfiguration;
        private readonly IProductImageService _productImageService;
        private readonly IFileService _fileService;
        private readonly ILogger<SellerProductService> _logger;

        private const string defaultCategoryName = "NEW PRODUCT UNKNOWN CATEGORY";

        public SellerProductService(ISellerProductDetailsRepository repository,
            IRepository<ProductDBModel> productRepository,
            IRepository<SellerDBModel> sellerRepository,
            IRepository<CategoryDBModel> categoryRepository,
            IRepository<PriceHistoryDBModel> priceHistoryRepository,
            IProductImageService productImageService,
            IFileService fileService,
            IOptions<SellerAccountConfiguration> options,
            ILogger<SellerProductService> logger)
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

                var product = (await _productRepository.GetFromConditionAsync(p =>
                    (!string.IsNullOrWhiteSpace(gtin) && (p.GTIN == gtin || p.UPC == gtin)) ||
                    (string.IsNullOrWhiteSpace(gtin) && p.NormalizedTitle.Equals(normalizedTitle))
                )).FirstOrDefault();


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

                    product = new ProductDBModel
                    {
                        GTIN = gtin,
                        Title = title,
                        Brand = offer.Element("brand")?.Value ?? string.Empty,
                        ModelNumber = offer.Element("model")?.Value ?? string.Empty,
                        Description = offer.Element("description")?.Value ?? string.Empty,
                        CategoryId = category?.Id ?? 0,
                        IsUnderModeration = true,
                        AddedToDatabase = DateTime.UtcNow,
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
                }


                // Price conversion to UAH
                var currencyId = offer.Element("currencyId")?.Value ?? string.Empty;
                var price = decimal.Parse(offer.Element("price")?.Value ?? "0");
                var priceUah = currencyId == "UAH" ? price : price * currencies[currencyId];

                // Write data to SellerProductDetails
                var details = (await _repository.GetFromConditionAsync(
                    d => d.ProductId == product.Id && d.SellerId == seller.UserId))
                    .FirstOrDefault();

                if (details == null)
                {
                    _ = await _repository.CreateAsync(new SellerProductDetailsDBModel
                    {
                        ProductId = product.Id,
                        SellerId = seller.UserId,
                        PriceValue = priceUah,
                        LastUpdated = priceListDate,
                        ProductStoreUrl = offer.Element("url")?.Value ?? string.Empty
                    });
                }
                else
                {
                    details.PriceValue = priceUah;
                    details.LastUpdated = priceListDate;
                    details.ProductStoreUrl = offer.Element("url")?.Value ?? string.Empty;
                    _ = await _repository.UpdateAsync(details);
                }

                // Write data to PriceHistory
                _ = await _priceHistoryRepository.CreateAsync(new PriceHistoryDBModel()
                {
                    ProductId = product.Id,
                    SellerId = seller.UserId,
                    CreatedAt = priceListDate,
                    PriceDate = priceListDate,
                    PriceValue = priceUah
                });
            }

            return OperationResultModel<string>.Success();
        }
    }
}
