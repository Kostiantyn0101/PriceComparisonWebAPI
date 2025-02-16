using System.Linq.Expressions;
using Domain.Models.DBModels;
using Domain.Models.Request.Seller;
using Domain.Models.Response;
using Domain.Models.Response.Seller;

namespace BLL.Services.SellerServices
{
    public interface ISellerService
    {
        Task<OperationResultModel<bool>> CreateAsync(SellerCreateRequestModel request);
        Task<OperationResultModel<bool>> UpdateAsync(SellerUpdateRequestModel request);
        Task<OperationResultModel<bool>> DeleteAsync(int id);
        IQueryable<SellerDBModel> GetQuery();
        Task<IEnumerable<SellerResponseModel>> GetFromConditionAsync(Expression<Func<SellerDBModel, bool>> condition);
        Task<IEnumerable<SellerDBModel>> ProcessQueryAsync(IQueryable<SellerDBModel> query);
    }
}
