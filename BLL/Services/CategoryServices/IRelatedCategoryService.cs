using System.Linq.Expressions;
using Domain.Models.DBModels;
using Domain.Models.Response;

namespace BLL.Services.CategoryService
{
    public interface IRelatedCategoryService
    {
        Task<OperationDetailsResponseModel> CreateAsync(RelatedCategoryDBModel model);
        Task<OperationDetailsResponseModel> UpdateAsync(RelatedCategoryDBModel entity);
        Task<OperationDetailsResponseModel> DeleteAsync(int categoryId, int relatedCategoryId);
        IQueryable<RelatedCategoryDBModel> GetQuery();
        Task<IEnumerable<RelatedCategoryDBModel>> GetFromConditionAsync(Expression<Func<RelatedCategoryDBModel, bool>> condition);
        Task<IEnumerable<RelatedCategoryDBModel>> ProcessQueryAsync(IQueryable<RelatedCategoryDBModel> query);
    }
}
