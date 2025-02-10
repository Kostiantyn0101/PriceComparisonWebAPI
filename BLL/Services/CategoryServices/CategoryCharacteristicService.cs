using System.Linq.Expressions;
using Azure.Core;
using DLL.Repository;
using DLL.Repository.Abstractions;
using Domain.Models.DBModels;
using Domain.Models.Exceptions;
using Domain.Models.Request.Categories;
using Domain.Models.Response;
using Microsoft.Extensions.Logging;

namespace BLL.Services.CategoryCharacteristicService
{
    public class CategoryCharacteristicService : ICategoryCharacteristicService
    {

        private readonly ICategoryCharacteristicRepository _categoryCharacteristicRepository;
        private readonly IRepository<CharacteristicDBModel> _characteristicRepository;

        public CategoryCharacteristicService(
            ICategoryCharacteristicRepository categoryCharacteristicRepository,
            IRepository<CharacteristicDBModel> characteristicRepository)
        {
            _categoryCharacteristicRepository = categoryCharacteristicRepository;
            _characteristicRepository = characteristicRepository;
        }

        public async Task<OperationDetailsResponseModel> CreateAsync(CategoryCharacteristicDBModel model)
        {
            var characteristicList = await _characteristicRepository.GetFromConditionAsync(x => x.Id == model.CharacteristicId);
            if (!characteristicList.Any())
            {
                return new OperationDetailsResponseModel
                {
                    IsError = true,
                    Message = $"Characteristic with ID {model.CharacteristicId} does not exist."
                };
            }

            var existingRecords = await GetFromConditionAsync(
                      x => x.CategoryId == model.CategoryId && x.CharacteristicId == model.CharacteristicId);
            if (existingRecords.Any())
            {
                return new OperationDetailsResponseModel
                {
                    IsError = true,
                    Message = $"Record with CategoryId = {model.CategoryId} and CharacteristicId = {model.CharacteristicId} already exists.",
                };
            }


            return await _categoryCharacteristicRepository.CreateAsync(model);
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
                results.Add(await CreateAsync(model));
            }

            return results;
        }

        public async Task<OperationDetailsResponseModel> UpdateAsync(CategoryCharacteristicDBModel entity)
        {
            return await _categoryCharacteristicRepository.UpdateAsync(entity);
        }

        public async Task<OperationDetailsResponseModel> DeleteAsync(int categoryId, int characteristicId)
        {
            return await _categoryCharacteristicRepository.DeleteAsync(categoryId, characteristicId);
        }

        public async Task<List<OperationDetailsResponseModel>> DeleteMultipleAsync(CategoryCharacteristicRequestModel request)
        {
            var results = new List<OperationDetailsResponseModel>();

            foreach (var id in request.CharacteristicIds)
            {
                results.Add(await _categoryCharacteristicRepository.DeleteAsync(request.CategoryId, id));
            }

            return results;
        }

        public IQueryable<CategoryCharacteristicDBModel> GetQuery()
        {
            return _categoryCharacteristicRepository.GetQuery();
        }

        public async Task<IEnumerable<CategoryCharacteristicDBModel>> GetFromConditionAsync(Expression<Func<CategoryCharacteristicDBModel, bool>> condition)
        {
            return await _categoryCharacteristicRepository.GetFromConditionAsync(condition);
        }

        public async Task<IEnumerable<CategoryCharacteristicDBModel>> ProcessQueryAsync(IQueryable<CategoryCharacteristicDBModel> query)
        {
            return await _categoryCharacteristicRepository.ProcessQueryAsync(query);
        }
    }
}
