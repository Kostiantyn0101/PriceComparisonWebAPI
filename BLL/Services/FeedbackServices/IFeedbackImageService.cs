using System.Linq.Expressions;
using Domain.Models.DBModels;
using Domain.Models.Request.Feedback;
using Domain.Models.Response;
using Domain.Models.Response.Feedback;

namespace BLL.Services.FeedbackAndReviewServices
{
    public interface IFeedbackImageService
    {
        Task<OperationResultModel<bool>> CreateAsync(FeedbackImageCreateRequestModel request);
        Task<OperationResultModel<FeedbackImageDBModel>> UpdateAsync(FeedbackImageDBModel entity);
        Task<OperationResultModel<bool>> DeleteAsync(FeedbackImageDeleteRequestModel request);
        Task<IEnumerable<FeedbackImageResponseModel>> GetFromConditionAsync(Expression<Func<FeedbackImageDBModel, bool>> condition);

        IQueryable<FeedbackImageDBModel> GetQuery();
        Task<IEnumerable<FeedbackImageDBModel>> ProcessQueryAsync(IQueryable<FeedbackImageDBModel> query);
    }
}
