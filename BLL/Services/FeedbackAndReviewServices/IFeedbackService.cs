using System.Linq.Expressions;
using Domain.Models.DBModels;
using Domain.Models.Response;

namespace BLL.Services.FeedbackAndReviewServices
{
    public interface IFeedbackService
    {
        Task<OperationDetailsResponseModel> CreateAsync(FeedbackDBModel model);
        Task<OperationDetailsResponseModel> UpdateAsync(FeedbackDBModel entity);
        Task<OperationDetailsResponseModel> DeleteAsync(int id);
        IQueryable<FeedbackDBModel> GetQuery();
        Task<IEnumerable<FeedbackDBModel>> GetFromConditionAsync(Expression<Func<FeedbackDBModel, bool>> condition);
        Task<IEnumerable<FeedbackDBModel>> ProcessQueryAsync(IQueryable<FeedbackDBModel> query);
    }
}
