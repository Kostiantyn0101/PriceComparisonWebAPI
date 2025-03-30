using System.Linq.Expressions;
using Domain.Models.DBModels;
using Domain.Models.Request.Seller;
using Domain.Models.Response;
using Domain.Models.Response.Seller;

namespace BLL.Services.SellerServices
{
    public interface ISellerRequestService
    {
        Task<OperationResultModel<SellerRequestDBModel>> CreateAsync(SellerRequestCreateRequestModel request);
        Task<OperationResultModel<SellerRequestDBModel>> UpdateAsync(SellerRequestUpdateRequestModel request);
        Task<OperationResultModel<bool>> DeleteAsync(int id);
        Task<OperationResultModel<SellerRequestDBModel>> ProcessRequestAsync(SellerRequestProcessRequestModel request);
        IQueryable<SellerRequestDBModel> GetQuery();
        Task<IEnumerable<SellerRequestResponseModel>> GetFromConditionAsync(Expression<Func<SellerRequestDBModel, bool>> condition);
        Task<IEnumerable<SellerRequestDBModel>> ProcessQueryAsync(IQueryable<SellerRequestDBModel> query);
    }
}
