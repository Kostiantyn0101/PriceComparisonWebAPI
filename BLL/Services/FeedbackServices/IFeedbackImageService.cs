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
        Task<OperationResultModel<bool>> DeleteAsync(FeedbackImageDeleteRequestModel request);
        Task<OperationDetailsResponseModel> UpdateAsync(FeedbackImageDBModel entity);
        IQueryable<FeedbackImageDBModel> GetQuery();
        Task<IEnumerable<FeedbackImageResponseModel>> GetFromConditionAsync(Expression<Func<FeedbackImageDBModel, bool>> condition);
        Task<IEnumerable<FeedbackImageDBModel>> ProcessQueryAsync(IQueryable<FeedbackImageDBModel> query);
    }
}
