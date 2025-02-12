using System.Linq.Expressions;
using Domain.Models.DBModels;
using Domain.Models.Request.Products;
using Domain.Models.Response;
using Domain.Models.Response.Products;

namespace BLL.Services.ProductServices
{
    public interface IReviewService
    {
        Task<OperationResultModel<bool>> CreateAsync(ReviewCreateRequestModel request);
        Task<OperationResultModel<bool>> UpdateAsync(ReviewUpdateRequestModel request);
        Task<OperationResultModel<bool>> DeleteAsync(int id);
        IQueryable<ReviewDBModel> GetQuery();
        Task<IEnumerable<ReviewResponseModel>> GetFromConditionAsync(Expression<Func<ReviewDBModel, bool>> condition);
        Task<IEnumerable<ReviewDBModel>> ProcessQueryAsync(IQueryable<ReviewDBModel> query);
    }
}
