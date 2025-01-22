using System.Linq.Expressions;
using DLL.Repository;
using Domain.Models.DBModels;
using Domain.Models.Response;

namespace BLL.Services
{
    public class PriceService : IPriceService
    {
        private readonly IPriceRepository _repository;

        public PriceService(IPriceRepository repository)
        {
            _repository = repository;
        }

        public async Task<OperationDetailsResponseModel> CreateAsync(PriceDBModel model)
        {
            return await _repository.CreateAsync(model);
        }

        public async Task<OperationDetailsResponseModel> UpdateAsync(PriceDBModel entity)
        {
            return await _repository.UpdateAsync(entity);
        }

        public async Task<OperationDetailsResponseModel> DeleteAsync(int productId, int sellerId)
        {
            return await _repository.DeleteAsync(productId, sellerId);
        }

        public IQueryable<PriceDBModel> GetQuery()
        {
            return _repository.GetQuery();
        }

        public async Task<IEnumerable<PriceDBModel>> GetFromConditionAsync(Expression<Func<PriceDBModel, bool>> condition)
        {
            return await _repository.GetFromConditionAsync(condition);
        }

        public async Task<IEnumerable<PriceDBModel>> ProcessQueryAsync(IQueryable<PriceDBModel> query)
        {
            return await _repository.ProcessQueryAsync(query);
        }
    }
}
