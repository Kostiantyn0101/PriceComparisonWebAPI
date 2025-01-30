using System.Linq.Expressions;
using DLL.Repository;
using Domain.Models.DBModels;
using Domain.Models.Response;

namespace BLL.Services.FeedbackAndReviewServices
{
    public class FeedbackService : IFeedbackService
    {
        private readonly IRepository<FeedbackDBModel> _repository;

        public FeedbackService(IRepository<FeedbackDBModel> repository)
        {
            _repository = repository;
        }

        public async Task<OperationDetailsResponseModel> CreateAsync(FeedbackDBModel model)
        {
            return await _repository.CreateAsync(model);
        }

        public async Task<OperationDetailsResponseModel> UpdateAsync(FeedbackDBModel entity)
        {
            return await _repository.UpdateAsync(entity);
        }

        public async Task<OperationDetailsResponseModel> DeleteAsync(int id)
        {
            return await _repository.DeleteAsync(id);
        }

        public IQueryable<FeedbackDBModel> GetQuery()
        {
            return _repository.GetQuery();
        }

        public async Task<IEnumerable<FeedbackDBModel>> GetFromConditionAsync(Expression<Func<FeedbackDBModel, bool>> condition)
        {
            return await _repository.GetFromConditionAsync(condition);
        }

        public async Task<IEnumerable<FeedbackDBModel>> ProcessQueryAsync(IQueryable<FeedbackDBModel> query)
        {
            return await _repository.ProcessQueryAsync(query);
        }
    }
}
