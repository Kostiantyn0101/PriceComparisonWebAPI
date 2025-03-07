using Domain.Models.DBModels;
using Domain.Models.Request.Filters;
using Domain.Models.Response;
using Domain.Models.Response.Filters;
using System.Linq.Expressions;

namespace BLL.Services.FilterServices
{
    public interface IProductFilterService
    {
        Task<OperationResultModel<ProductFilterDBModel>> CreateAsync(ProductFilterCreateRequestModel request);
        Task<OperationResultModel<ProductFilterDBModel>> UpdateAsync(ProductFilterUpdateRequestModel request);
        Task<OperationResultModel<bool>> DeleteAsync(int productId, int filterId);
        IQueryable<ProductFilterDBModel> GetQuery();
        Task<IEnumerable<ProductFilterResponseModel>> GetFromConditionAsync(Expression<Func<ProductFilterDBModel, bool>> condition);
        Task<IEnumerable<ProductFilterDBModel>> ProcessQueryAsync(IQueryable<ProductFilterDBModel> query);
    }
}
