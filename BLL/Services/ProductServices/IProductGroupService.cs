using System.Linq.Expressions;
using Domain.Models.DBModels;
using Domain.Models.Request.Products;
using Domain.Models.Response;
using Domain.Models.Response.Products;

namespace BLL.Services.ProductServices
{
    public interface IProductGroupService
    {
        Task<OperationResultModel<ProductGroupDBModel>> CreateAsync(ProductGroupCreateRequestModel request);
        Task<OperationResultModel<ProductGroupDBModel>> UpdateAsync(ProductGroupUpdateRequestModel request);
        Task<OperationResultModel<bool>> DeleteAsync(int id);
        IQueryable<ProductGroupDBModel> GetQuery();
        Task<IEnumerable<ProductGroupResponseModel>> GetFromConditionAsync(Expression<Func<ProductGroupDBModel, bool>> condition);
        Task<IEnumerable<ProductGroupDBModel>> ProcessQueryAsync(IQueryable<ProductGroupDBModel> query);
    }

}
