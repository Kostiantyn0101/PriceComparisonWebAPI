using System.Linq;
using DLL.Repository;
using Domain.Models.Configuration;
using Domain.Models.DBModels;
using Domain.Models.Extensions;
using Domain.Models.Response.Categories;
using Domain.Models.Response.Products;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace BLL.Services.ProductServices
{
    public class PopularProductSercice : IPopularProductService
    {
        private readonly PopularProductsSettings _settings;
        private readonly FileStorageConfiguration _storageSettings;
        private readonly IRepository<ProductClicksDBModel, int> _productClickRepository;
        private readonly ILogger<PopularProductSercice> _logger;
        private readonly IRepository<ProductDBModel, int> _producRepository;


        public PopularProductSercice(
            IRepository<ProductClicksDBModel, int> productClickRepository,
            IRepository<ProductDBModel, int> producRepository,
            ILogger<PopularProductSercice> logger,
            IOptions<PopularProductsSettings> options,
            IOptions<FileStorageConfiguration> storageOptions)
        {
            _productClickRepository = productClickRepository;
            _producRepository = producRepository;
            _logger = logger;
            _settings = options.Value;
            _storageSettings = storageOptions.Value;
        }


        public async Task RegisterClickAsync(int productId)
        {
            var dbModel = new ProductClicksDBModel() { ProductId = productId, ClickDate = DateTime.UtcNow };
            var result = await _productClickRepository.CreateAsync(dbModel);

            if (!result.IsSuccess)
            {
                _logger.LogError(result.ErrorMessage, result.Exception);
            }
        }

        public async Task<IEnumerable<PopularCategoryResponseModel>> GetPopularCategories()
        {
            var lookupTime = DateTime.UtcNow.AddMonths(-_settings.LookbackMonths);

            var topCategoriesQuery = _productClickRepository.GetQuery()
                .Where(pc => pc.ClickDate >= lookupTime)
                .GroupBy(pc => pc.Product.Category)
                .OrderByDescending(g => g.Count())
                .Take(_settings.PopularCategoriesCount)
                .Select(g => new PopularCategoryResponseModel
                {
                    Id = g.Key.Id,
                    Title = g.Key.Title
                });

            var topCategories = await topCategoriesQuery.ToListAsync();
            return topCategories;
        }

        public async Task<IEnumerable<PopularProductResponseModel>> GetPopularProductsByCategory(int categoryId)
        {
            var lookupTime = DateTime.UtcNow.AddMonths(-_settings.LookbackMonths);

            var topProductsQuery = _productClickRepository.GetQuery()
                .Where(pc =>
                    pc.ClickDate >= lookupTime &&
                    pc.Product.Category.Id == categoryId)
                .GroupBy(pc => pc.Product)
                .Select(g => new
                {
                    Product = g.Key,
                    Clicks = g.Count()
                })
                .OrderByDescending(g => g.Clicks)
                .Take(_settings.PopularProductsPerCategoryCount)
                .Select(x => new PopularProductResponseModel
                {
                    Id = x.Product.Id,
                    Title = x.Product.Title,
                    ImageUrl = x.Product.ProductImages
                        .Where(pi => pi.IsPrimary)
                        .Select(pi => pi.ImageUrl)
                        .FirstOrDefault() ?? x.Product.ProductImages
                            .OrderBy(pi => pi.Id)
                            .Select(pi => pi.ImageUrl)
                            .FirstOrDefault(),
                    MinPrice = x.Product.Prices.Min(p => p.PriceValue)
                });

            var topProducts = await topProductsQuery.ToListAsync();
            return topProducts;
        }

        public async Task<IEnumerable<PopularCategoryResponseModel>> GetPopularCategoriesWithProducts()
        {
            var lookupTime = DateTime.UtcNow.AddMonths(-_settings.LookbackMonths);

            var topCategoriesQuery = _productClickRepository.GetQuery()
                .Where(pc => pc.ClickDate >= lookupTime)
                .GroupBy(pc => new { pc.Product.Category.Id, pc.Product.Category.Title })
                .Select(g => new
                {
                    CategoryId = g.Key.Id,
                    CategoryTitle = g.Key.Title,
                    TotalClicks = g.Count()
                })
                .OrderByDescending(c => c.TotalClicks)
                .Take(_settings.PopularCategoriesCount);

            var topCategoriesWithProductsQuery = topCategoriesQuery.Select(c => new PopularCategoryResponseModel
            {
                Id = c.CategoryId,
                Title = c.CategoryTitle,
                Products = _producRepository.GetQuery()
                    .Where(p => p.Category.Id == c.CategoryId)
                    .Select(p => new
                    {
                        Product = p,
                        Clicks = p.ProductClicks.Count(pc => pc.ClickDate >= lookupTime)
                    })
                    .OrderByDescending(p => p.Clicks)
                    .Take(_settings.PopularProductsPerCategoryCount)
                    .Select(p => new PopularProductResponseModel
                    {
                        Id = p.Product.Id,
                        Title = p.Product.Title,
                        ImageUrl = p.Product.ProductImages
                            .Where(pi => pi.IsPrimary)
                            .Select(pi => pi.ImageUrl)
                            .FirstOrDefault() ?? p.Product.ProductImages
                                .OrderBy(pi => pi.Id)
                                .Select(pi => pi.ImageUrl)
                                .FirstOrDefault(),
                        MinPrice = p.Product.Prices.Any()
                            ? p.Product.Prices.Min(pr => pr.PriceValue)
                            : 0
                    })
                    .ToList()

            });

            var topCategoriesWithProducts = await topCategoriesWithProductsQuery.ToListAsync();
            
            // Add server url to product image path
            topCategoriesWithProducts.ForEach(c => c.Products?.ToList().ForEach(p => p.ApplyServerUrl(_storageSettings.ServerURL)));
            
            return topCategoriesWithProducts;
        }
    }
}
