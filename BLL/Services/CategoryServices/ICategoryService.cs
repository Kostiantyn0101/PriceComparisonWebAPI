using Domain.Models.DBModels;
using Domain.Models.Request.Categories;
using Domain.Models.Response;
using Domain.Models.Response.Categories;
using System.Linq.Expressions;

namespace BLL.Services.CategoryService
{
    public interface ICategoryService
    {
        Task<OperationResultModel<CategoryDBModel>> CreateAsync(CategoryCreateRequestModel model);
        Task<OperationResultModel<CategoryDBModel>> UpdateAsync(CategoryUpdateRequestModel entity);
        Task<OperationResultModel<bool>> DeleteAsync(int id);
        IQueryable<CategoryDBModel> GetQuery();
        Task<IEnumerable<CategoryResponseModel>> GetFromConditionAsync(Expression<Func<CategoryDBModel, bool>> condition);
        Task<IEnumerable<CategoryDBModel>> ProcessQueryAsync(IQueryable<CategoryDBModel> query);
    }
}
