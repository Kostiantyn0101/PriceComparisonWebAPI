using System.Linq.Expressions;
using Domain.Models.DBModels;
using Domain.Models.Request.Categories;
using Domain.Models.Response;
using Domain.Models.Response.Categories;

namespace BLL.Services.CategoryService
{
    public interface IRelatedCategoryService
    {
        Task<OperationResultModel<bool>> CreateAsync(RelatedCategoryRequestModel model);
        Task<OperationResultModel<bool>> UpdateAsync(RelatedCategoryUpdateRequestModel entity);
        Task<OperationResultModel<bool>> DeleteAsync(int categoryId, int relatedCategoryId);
        IQueryable<RelatedCategoryDBModel> GetQuery();
        Task<IEnumerable<RelatedCategoryResponseModel>> GetFromConditionAsync(Expression<Func<RelatedCategoryDBModel, bool>> condition);
        Task<IEnumerable<RelatedCategoryDBModel>> ProcessQueryAsync(IQueryable<RelatedCategoryDBModel> query);
    }
}
