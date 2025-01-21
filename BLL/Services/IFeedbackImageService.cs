using System.Linq.Expressions;
using Domain.Models.DBModels;
using Domain.Models.Response;

namespace BLL.Services
{
    public interface IFeedbackImageService
    {
        Task<OperationDetailsResponseModel> CreateAsync(FeedbackImageDBModel model);
        Task<OperationDetailsResponseModel> UpdateAsync(FeedbackImageDBModel entity);
        Task<OperationDetailsResponseModel> DeleteAsync(int id);
        IQueryable<FeedbackImageDBModel> GetQuery();
        Task<IEnumerable<FeedbackImageDBModel>> GetFromConditionAsync(Expression<Func<FeedbackImageDBModel, bool>> condition);
        Task<IEnumerable<FeedbackImageDBModel>> ProcessQueryAsync(IQueryable<FeedbackImageDBModel> query);
    }
}
