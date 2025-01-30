using System.Linq.Expressions;
using DLL.Repository;
using Domain.Models.DBModels;
using Domain.Models.Response;

namespace BLL.Services.FeedbackAndReviewServices
{
    public class FeedbackImageService : IFeedbackImageService
    {
        private readonly IRepository<FeedbackImageDBModel> _repository;

        public FeedbackImageService(IRepository<FeedbackImageDBModel> repository)
        {
            _repository = repository;
        }

        public async Task<OperationDetailsResponseModel> CreateAsync(FeedbackImageDBModel model)
        {
            return await _repository.CreateAsync(model);
        }

        public async Task<OperationDetailsResponseModel> UpdateAsync(FeedbackImageDBModel entity)
        {
            return await _repository.UpdateAsync(entity);
        }

        public async Task<OperationDetailsResponseModel> DeleteAsync(int id)
        {
            return await _repository.DeleteAsync(id);
        }

        public IQueryable<FeedbackImageDBModel> GetQuery()
        {
            return _repository.GetQuery();
        }

        public async Task<IEnumerable<FeedbackImageDBModel>> GetFromConditionAsync(Expression<Func<FeedbackImageDBModel, bool>> condition)
        {
            return await _repository.GetFromConditionAsync(condition);
        }

        public async Task<IEnumerable<FeedbackImageDBModel>> ProcessQueryAsync(IQueryable<FeedbackImageDBModel> query)
        {
            return await _repository.ProcessQueryAsync(query);
        }
    }
}
