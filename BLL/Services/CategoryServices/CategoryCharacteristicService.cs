using System.Linq.Expressions;
using Azure.Core;
using DLL.Repository;
using Domain.Models.DBModels;
using Domain.Models.Request.Categories;
using Domain.Models.Response;
using Microsoft.Extensions.Logging;

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

        public async Task<List<OperationDetailsResponseModel>> CreateMultipleAsync(CategoryCharacteristicRequestModel request)
        {
            var models = request.CharacteristicIds.Select(id => new CategoryCharacteristicDBModel
                   {
                       CategoryId = request.CategoryId,
                       CharacteristicId = id
                   }).ToList();

            var results = new List<OperationDetailsResponseModel>();

            foreach (var model in models)
            {
                results.Add(await _repository.CreateAsync(model));
            }

            return results;
        }

        public async Task<OperationDetailsResponseModel> UpdateAsync(CategoryCharacteristicDBModel entity)
        {
            return await _repository.UpdateAsync(entity);
        }

        public async Task<OperationDetailsResponseModel> DeleteAsync(int categoryId, int characteristicId)
        {
            return await _repository.DeleteAsync(categoryId, characteristicId);
        }

        public async Task<List<OperationDetailsResponseModel>> DeleteMultipleAsync(CategoryCharacteristicRequestModel request)
        {
            var results = new List<OperationDetailsResponseModel>();

            foreach (var id in request.CharacteristicIds)
            {
                results.Add(await _repository.DeleteAsync(request.CategoryId, id));
            }

            return results;
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
