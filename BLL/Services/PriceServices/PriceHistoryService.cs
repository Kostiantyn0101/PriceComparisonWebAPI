using System.Linq.Expressions;
using DLL.Repository;
using Domain.Models.DBModels;
using Domain.Models.Response;

namespace BLL.Services.PriceServices
{
    public class PriceHistoryService : IPriceHistoryService
    {
        private readonly IRepository<PriceHistoryDBModel, int> _repository;

        public PriceHistoryService(IRepository<PriceHistoryDBModel, int> repository)
        {
            _repository = repository;
        }

        public async Task<OperationResultModel<PriceHistoryDBModel>> CreateAsync(PriceHistoryDBModel model)
        {
            return await _repository.CreateAsync(model);
        }

        public async Task<OperationResultModel<PriceHistoryDBModel>> UpdateAsync(PriceHistoryDBModel entity)
        {
            return await _repository.UpdateAsync(entity);
        }

        public async Task<OperationResultModel<bool>> DeleteAsync(int id)
        {
            return await _repository.DeleteAsync(id);
        }

        public IQueryable<PriceHistoryDBModel> GetQuery()
        {
            return _repository.GetQuery();
        }

        public async Task<IEnumerable<PriceHistoryDBModel>> GetFromConditionAsync(Expression<Func<PriceHistoryDBModel, bool>> condition)
        {
            return await _repository.GetFromConditionAsync(condition);
        }

        public async Task<IEnumerable<PriceHistoryDBModel>> ProcessQueryAsync(IQueryable<PriceHistoryDBModel> query)
        {
            return await _repository.ProcessQueryAsync(query);
        }
    }
}
