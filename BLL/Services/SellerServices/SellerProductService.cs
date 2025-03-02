using Domain.Models.DBModels;
using Microsoft.Extensions.Options;
using System.Xml.Linq;
using DLL.Repository;
using Domain.Models.Configuration;
using Domain.Models.Response;
using OpenAI.Moderations;
using BLL.Services.ProductServices;

namespace BLL.Services.SellerServices
{
    public class SellerProductService : ISellerProductService
    {
        private readonly ISellerProductDetailsRepository _repository;
        private readonly IRepository<ProductDBModel> _productRepository;
        private readonly IRepository<SellerDBModel> _sellerRepository;
        private readonly IRepository<CategoryDBModel> _categoryRepository;
        private readonly SellerAccountConfiguration _accountConfiguration;
        private readonly IProductImageService _productImageService;

        public SellerProductService(ISellerProductDetailsRepository repository,
            IRepository<ProductDBModel> productRepository,
            IRepository<SellerDBModel> sellerRepository,
            IRepository<CategoryDBModel> categoryRepository,
            IProductImageService productImageService,
        IOptions<SellerAccountConfiguration> options)
        {
            _repository = repository;
            _productRepository = productRepository;
            _sellerRepository = sellerRepository;
            _categoryRepository = categoryRepository;
            _accountConfiguration = options.Value;
            _productImageService = productImageService;
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
            Dictionary<int, string> categories = xmlDoc.Root?.Element("categories") is XElement categoriesElement
                && categoriesElement.Elements("category").Any()
                ? categoriesElement.Elements("category")
                   .ToDictionary(
                        c => int.Parse(c.Attribute("id")?.Value ?? "0"),  // Key: ID
                        c => c.Value.Trim()  // Value: Category name
                    )
                : new Dictionary<int, string>();

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
                    var categoryNormalizedTitle = offer.Element("category")?.Value.Trim().ToUpperInvariant() ?? string.Empty;
                    var category = (await _categoryRepository.GetFromConditionAsync(c => c.Title.Trim().ToUpperInvariant() == normalizedTitle)).FirstOrDefault();

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

                    await _productRepository.CreateAsync(product);
                }

                // Конвертация цены
                var currencyId = offer.Element("currencyId").Value;
                var price = decimal.Parse(offer.Element("price").Value);
                var priceUah = currencyId == "UAH" ? price : price * currencies[currencyId];

                // Работа с SellerProductDetails
                var details = (await _repository.GetFromConditionAsync(
                    d => d.ProductId == product.Id && d.SellerId == seller.UserId))
                    .FirstOrDefault();

                if (details == null)
                {
                    await _repository.CreateAsync(new SellerProductDetailsDBModel
                    {
                        ProductId = product.Id,
                        SellerId = seller.UserId,
                        PriceValue = priceUah,
                        LastUpdated = priceListDate,
                        ProductStoreUrl = offer.Element("url").Value
                    });
                }
                else
                {
                    details.PriceValue = priceUah;
                    details.LastUpdated = priceListDate;
                    details.ProductStoreUrl = offer.Element("url").Value;
                    await _repository.UpdateAsync(details);
                }
            }

            return OperationResultModel<string>.Success();
        }
    }
}
