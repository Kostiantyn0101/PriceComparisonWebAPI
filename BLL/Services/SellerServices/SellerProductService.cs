using Domain.Models.DBModels;
using Microsoft.Extensions.Options;
using System.Xml.Linq;
using DLL.Repository;
using Domain.Models.Configuration;
using Domain.Models.Response;

namespace BLL.Services.SellerServices
{
    public class SellerProductService : ISellerProductService
    {
        private readonly ISellerProductDetailsRepository _repository;
        private readonly IRepository<ProductDBModel> _productRepository;
        private readonly IRepository<SellerDBModel> _sellerRepository;
        private readonly SellerAccountConfiguration _accountConfiguration;

        public SellerProductService(ISellerProductDetailsRepository repository,
            IRepository<ProductDBModel> productRepository,
            IRepository<SellerDBModel> sellerRepository, IOptions<SellerAccountConfiguration> options)
        {
            _repository = repository;
            _productRepository = productRepository;
            _sellerRepository = sellerRepository;
            _accountConfiguration = options.Value;
        }

        public async Task<OperationResultModel<string>> ProcessXmlAsync(Stream stream)
        {
            var xmlDoc = await XDocument.LoadAsync(stream, LoadOptions.None, CancellationToken.None);
            var priceListDate = DateTime.Parse(xmlDoc.Root.Attribute("date").Value);
            var apiKey = xmlDoc.Root.Element("api_key").Value;

            // Поиск продавца
            var seller = (await _sellerRepository.GetFromConditionAsync(s => s.ApiKey == apiKey))
                .FirstOrDefault();

            if (seller == null || !seller.IsActive || seller.AccountBalance < _accountConfiguration.MinBalanceToProceed)
            {
                throw new Exception("Invalid seller");
            }

            // Обработка валют
            var currencies = xmlDoc.Root.Element("currencies").Elements("currency")
                .ToDictionary(
                    c => c.Attribute("id").Value,
                    c => decimal.Parse(c.Attribute("rate").Value)
                );

            // Обработка товаров
            foreach (var offer in xmlDoc.Root.Element("offers").Elements("offer"))
            {
                var gtin = offer.Element("gtin").Value;

                // Поиск товара
                var product = (await _productRepository.GetFromConditionAsync(p => p.GTIN == gtin || p.UPC == gtin)).FirstOrDefault();

                // Создание нового товара если не найден
                if (product == null)
                {
                    //product = new ProductDBModel
                    //{
                    //    GTIN = gtin,
                    //    Name = offer.Element("name").Value,
                    //    Brand = offer.Element("brand").Value,
                    //    Model = offer.Element("model").Value,
                    //    Description = offer.Element("description").Value,
                    //    CategoryId = int.Parse(offer.Element("categoryId").Value),
                    //    AddedToDatabase = DateTime.UtcNow
                    //};

                    //await _productRepository.AddAsync(product);

                    continue;
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
