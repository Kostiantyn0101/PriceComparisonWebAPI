using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Models.DBModels;
using Microsoft.Extensions.Options;
using System.Xml.Linq;
using DLL.Repository;
using BLL.Services.ProductServices;
using Domain.Models.Configuration;

namespace BLL.Services.SellerServices
{
    public class SellerProductService : ISellerProductService
    {
        private readonly IRepository<SellerProductDetailsDBModel> _repository;
        private readonly IRepository<ProductDBModel> _productRepository;
        private readonly IRepository<SellerDBModel> _sellerRepository;
        private readonly SellerAccountConfiguration _accountConfiguration;

        public SellerProductService(IRepository<SellerProductDetailsDBModel> repository, 
            IRepository<ProductDBModel> productRepository, 
            IRepository<SellerDBModel> sellerRepository, IOptions<SellerAccountConfiguration> options)
        {
            _repository = repository;
            _productRepository = productRepository;
            _sellerRepository = sellerRepository;
            _accountConfiguration = options.Value;
        }

        public async Task ProcessXmlAsync(string xmlContent)
        {
            //    var doc = XDocument.Parse(xmlContent);
            //    var apiKey = doc.Root?.Element("api_key")?.Value;
            //    var seller = (await _sellerRepository.GetFromConditionAsync(s => s.ApiKey == apiKey)).FirstOrDefault();

            //    if (seller == null || !seller.IsActive || seller.AccountBalance < _minimumBalance)
            //    {
            //        throw new Exception("Invalid or insufficient seller account.");
            //    }

            //    var currencyRates = doc.Root?.Element("currencies")?.Elements("currency")
            //        .ToDictionary(c => c.Attribute("id")?.Value, c => decimal.Parse(c.Attribute("rate")?.Value, CultureInfo.InvariantCulture));

            //    var offers = doc.Root?.Element("offers")?.Elements("offer");
            //    foreach (var offer in offers)
            //    {
            //        var gtin = offer.Element("gtin")?.Value;
            //        var product = await _context.Products.FirstOrDefaultAsync(p => p.GTIN == gtin || p.UPC == gtin);

            //        if (product == null)
            //        {
            //            product = new ProductDBModel
            //            {
            //                GTIN = gtin,
            //                Name = offer.Element("name")?.Value,
            //                Brand = offer.Element("brand")?.Value,
            //                CreatedAt = DateTime.UtcNow
            //            };
            //            _context.Products.Add(product);
            //            await _context.SaveChangesAsync();
            //        }

            //        var priceValue = decimal.Parse(offer.Element("price")?.Value, CultureInfo.InvariantCulture);
            //        var currencyId = offer.Element("currencyId")?.Value;
            //        if (currencyRates.ContainsKey(currencyId))
            //        {
            //            priceValue *= currencyRates[currencyId];
            //        }

            //        var productStoreUrl = offer.Element("url")?.Value;

            //        var priceHistory = new PriceHistoryDBModel
            //        {
            //            ProductId = product.Id,
            //            SellerId = seller.Id,
            //            PriceValue = priceValue,
            //            CreatedAt = DateTime.UtcNow
            //        };
            //        _context.PriceHistories.Add(priceHistory);

            //        var existingDetails = await _context.SellerProductDetails.FirstOrDefaultAsync(p => p.ProductId == product.Id && p.SellerId == seller.Id);
            //        if (existingDetails != null)
            //        {
            //            existingDetails.PriceValue = priceValue;
            //            existingDetails.LastUpdated = DateTime.UtcNow;
            //            existingDetails.ProductStoreUrl = productStoreUrl;
            //        }
            //        else
            //        {
            //            _context.SellerProductDetails.Add(new SellerProductDetailsDBModel
            //            {
            //                ProductId = product.Id,
            //                SellerId = seller.Id,
            //                PriceValue = priceValue,
            //                LastUpdated = DateTime.UtcNow,
            //                ProductStoreUrl = productStoreUrl
            //            });
            //        }
            //    }
            //    await _context.SaveChangesAsync();
        }
    }
}
