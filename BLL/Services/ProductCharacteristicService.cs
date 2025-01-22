using System.Linq.Expressions;
using DLL.Repository;
using Domain.Models.DBModels;
using Domain.Models.Response;

namespace BLL.Services
{
    public class ProductCharacteristicService : IProductCharacteristicService
    {
        private readonly IProductCharacteristicRepository _repository;

        public ProductCharacteristicService(IProductCharacteristicRepository repository)
        {
            _repository = repository;
        }

        public async Task<OperationDetailsResponseModel> CreateAsync(ProductCharacteristicDBModel model)
        {
            return await _repository.CreateAsync(model);
        }

        public async Task<OperationDetailsResponseModel> UpdateAsync(ProductCharacteristicDBModel entity)
        {
            return await _repository.UpdateAsync(entity);
        }

        public async Task<OperationDetailsResponseModel> DeleteAsync(int productId, int characteristicId)
        {
            return await _repository.DeleteAsync(productId, characteristicId);
        }

        public IQueryable<ProductCharacteristicDBModel> GetQuery()
        {
            return _repository.GetQuery();
        }

        public async Task<IEnumerable<ProductCharacteristicDBModel>> GetFromConditionAsync(Expression<Func<ProductCharacteristicDBModel, bool>> condition)
        {
            return await _repository.GetFromConditionAsync(condition);
        }

        public async Task<IEnumerable<ProductCharacteristicDBModel>> ProcessQueryAsync(IQueryable<ProductCharacteristicDBModel> query)
        {
            return await _repository.ProcessQueryAsync(query);
        }
    }
}
