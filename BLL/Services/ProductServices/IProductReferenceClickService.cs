using System.Linq.Expressions;
using Domain.Models.DBModels;
using Domain.Models.Request.Products;
using Domain.Models.Response;
using Domain.Models.Response.Products;

namespace BLL.Services.ProductServices
{
    public interface IProductReferenceClickService
    {
        Task<OperationResultModel<bool>> ProcessReferenceClick(ProductSellerReferenceClickCreateRequestModel request);
        Task<OperationResultModel<bool>> UpdateAsync(ProductSellerReferenceClickUpdateRequestModel request);
        Task<OperationResultModel<bool>> DeleteAsync(int id);
        IQueryable<ProductReferenceClickDBModel> GetQuery();
        Task<IEnumerable<ProductSellerReferenceClickResponseModel>> GetFromConditionAsync(Expression<Func<ProductReferenceClickDBModel, bool>> condition);
        Task<IEnumerable<ProductReferenceClickDBModel>> ProcessQueryAsync(IQueryable<ProductReferenceClickDBModel> query);
    }
}
