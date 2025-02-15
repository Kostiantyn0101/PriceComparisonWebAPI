using DLL.Repository;
using Domain.Models.DBModels;
using Microsoft.Extensions.Logging;

namespace BLL.Services.ProductServices
{
    public class PopularProductSercice : IIPopularProductService
    {
        private readonly IRepository<ProductClicksDBModel> _repository;
        private readonly ILogger<PopularProductSercice> _logger;

        public PopularProductSercice(IRepository<ProductClicksDBModel> repository, ILogger<PopularProductSercice> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task RegisterClickAsync(int productId)
        {
            var dbModel = new ProductClicksDBModel() { ProductId = productId, ClickDate = DateTime.UtcNow };
            var result = await _repository.CreateAsync(dbModel);

            if (result.IsError)
            {
                _logger.LogError(result.Message, result.Exception);
            }
        }
    }
}
