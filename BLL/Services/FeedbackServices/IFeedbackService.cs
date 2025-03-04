using System.Linq.Expressions;
using Domain.Models.DBModels;
using Domain.Models.Request.Feedback;
using Domain.Models.Response;
using Domain.Models.Response.Feedback;

namespace BLL.Services.FeedbackAndReviewServices
{
    public interface IFeedbackService
    {
        Task<OperationResultModel<FeedbackDBModel>> CreateAsync(FeedbackCreateRequestModel request);
        Task<OperationResultModel<FeedbackDBModel>> UpdateAsync(FeedbackUpdateRequestModel request);
        Task<OperationResultModel<bool>> DeleteAsync(int id);
        Task<IEnumerable<FeedbackResponseModel>> GetFromConditionAsync(Expression<Func<FeedbackDBModel, bool>> condition);

        IQueryable<FeedbackDBModel> GetQuery();
        Task<IEnumerable<FeedbackDBModel>> ProcessQueryAsync(IQueryable<FeedbackDBModel> query);
    }
}
