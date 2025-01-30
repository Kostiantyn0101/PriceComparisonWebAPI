using System.Linq.Expressions;
using Domain.Models.DBModels;
using Domain.Models.Response;

namespace BLL.Services.FeedbackAndReviewServices
{
    public interface IReviewService
    {
        Task<OperationDetailsResponseModel> CreateAsync(ReviewDBModel model);
        Task<OperationDetailsResponseModel> UpdateAsync(ReviewDBModel entity);
        Task<OperationDetailsResponseModel> DeleteAsync(int id);
        IQueryable<ReviewDBModel> GetQuery();
        Task<IEnumerable<ReviewDBModel>> GetFromConditionAsync(Expression<Func<ReviewDBModel, bool>> condition);
        Task<IEnumerable<ReviewDBModel>> ProcessQueryAsync(IQueryable<ReviewDBModel> query);
    }
}
