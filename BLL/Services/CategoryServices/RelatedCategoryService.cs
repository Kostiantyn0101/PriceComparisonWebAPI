using System.Linq.Expressions;
using DLL.Repository;
using Domain.Models.DBModels;
using Domain.Models.Response;

namespace BLL.Services.CategoryService
{
    public class RelatedCategoryService : IRelatedCategoryService
    {
        private readonly IRelatedCategoryRepository _repository;

        public RelatedCategoryService(IRelatedCategoryRepository repository)
        {
            _repository = repository;
        }

        public async Task<OperationDetailsResponseModel> CreateAsync(RelatedCategoryDBModel model)
        {
            return await _repository.CreateAsync(model);
        }

        public async Task<OperationDetailsResponseModel> UpdateAsync(RelatedCategoryDBModel entity)
        {
            return await _repository.UpdateAsync(entity);
        }

        public async Task<OperationDetailsResponseModel> DeleteAsync(int categoryId, int relatedCategoryId)
        {
            return await _repository.DeleteAsync(categoryId, relatedCategoryId);
        }

        public IQueryable<RelatedCategoryDBModel> GetQuery()
        {
            return _repository.GetQuery();
        }

        public async Task<IEnumerable<RelatedCategoryDBModel>> GetFromConditionAsync(Expression<Func<RelatedCategoryDBModel, bool>> condition)
        {
            return await _repository.GetFromConditionAsync(condition);
        }

        public async Task<IEnumerable<RelatedCategoryDBModel>> ProcessQueryAsync(IQueryable<RelatedCategoryDBModel> query)
        {
            return await _repository.ProcessQueryAsync(query);
        }
    }
}
