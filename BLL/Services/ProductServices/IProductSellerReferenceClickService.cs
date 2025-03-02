using System.Linq.Expressions;
using Domain.Models.DBModels;
using Domain.Models.Request.Products;
using Domain.Models.Response;
using Domain.Models.Response.Products;

namespace BLL.Services.ProductServices
{
    public interface IProductSellerReferenceClickService
    {
        Task<OperationResultModel<bool>> ProcessReferenceClick(ProductSellerReferenceClickCreateRequestModel request);
        Task<OperationResultModel<bool>> UpdateAsync(ProductSellerReferenceClickUpdateRequestModel request);
        Task<OperationResultModel<bool>> DeleteAsync(int id);
        IQueryable<ProductSellerReferenceClickDBModel> GetQuery();
        Task<IEnumerable<ProductSellerReferenceClickResponseModel>> GetFromConditionAsync(Expression<Func<ProductSellerReferenceClickDBModel, bool>> condition);
        Task<IEnumerable<ProductSellerReferenceClickDBModel>> ProcessQueryAsync(IQueryable<ProductSellerReferenceClickDBModel> query);
    }
}
