using Domain.Models.DBModels;
using Domain.Models.Response;
using System.Linq.Expressions;

namespace DLL.Repository
{
    //deprecated
    public interface IRelatedCategoryRepository
    {
        Task<OperationDetailsResponseModel> CreateAsync(RelatedCategoryDBModel entity);
        Task<OperationDetailsResponseModel> UpdateAsync(RelatedCategoryDBModel entityNew);
        Task<OperationDetailsResponseModel> DeleteAsync(int categoryId, int relatedCategoryId);

        Task<IEnumerable<RelatedCategoryDBModel>> GetFromConditionAsync(Expression<Func<RelatedCategoryDBModel, bool>> condition);
        Task<IEnumerable<RelatedCategoryDBModel>> ProcessQueryAsync(IQueryable<RelatedCategoryDBModel> query);
        IQueryable<RelatedCategoryDBModel> GetQuery();
    }
}
