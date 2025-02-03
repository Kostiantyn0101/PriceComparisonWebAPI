using System.Linq.Expressions;
using DLL.Repository;
using Domain.Models.DBModels;
using Domain.Models.Response;

namespace BLL.Services.CategoryCharacteristicService
{
    public class CategoryCharacteristicService : ICategoryCharacteristicService
    {
        private readonly ICategoryCharacteristicRepository _repository;

        public CategoryCharacteristicService(ICategoryCharacteristicRepository repository)
        {
            _repository = repository;
        }

        public async Task<OperationDetailsResponseModel> CreateAsync(CategoryCharacteristicDBModel model)
        {
            return await _repository.CreateAsync(model);
        }

        public async Task<OperationDetailsResponseModel> UpdateAsync(CategoryCharacteristicDBModel entity)
        {
            return await _repository.UpdateAsync(entity);
        }

        public async Task<OperationDetailsResponseModel> DeleteAsync(int categoryId, int characteristicId)
        {
            return await _repository.DeleteAsync(categoryId, characteristicId);
        }

        public IQueryable<CategoryCharacteristicDBModel> GetQuery()
        {
            return _repository.GetQuery();
        }

        public async Task<IEnumerable<CategoryCharacteristicDBModel>> GetFromConditionAsync(Expression<Func<CategoryCharacteristicDBModel, bool>> condition)
        {
            return await _repository.GetFromConditionAsync(condition);
        }

        public async Task<IEnumerable<CategoryCharacteristicDBModel>> ProcessQueryAsync(IQueryable<CategoryCharacteristicDBModel> query)
        {
            return await _repository.ProcessQueryAsync(query);
        }
    }
}
