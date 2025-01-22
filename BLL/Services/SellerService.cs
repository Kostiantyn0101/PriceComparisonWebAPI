using System.Linq.Expressions;
using DLL.Repository;
using Domain.Models.DBModels;
using Domain.Models.Response;

namespace BLL.Services
{
    public class SellerService : ISellerService
    {
        private readonly IRepository<SellerDBModel> _repository;

        public SellerService(IRepository<SellerDBModel> repository)
        {
            _repository = repository;
        }

        public async Task<OperationDetailsResponseModel> CreateAsync(SellerDBModel model)
        {
            return await _repository.CreateAsync(model);
        }

        public async Task<OperationDetailsResponseModel> UpdateAsync(SellerDBModel entity)
        {
            return await _repository.UpdateAsync(entity);
        }

        public async Task<OperationDetailsResponseModel> DeleteAsync(int id)
        {
            return await _repository.DeleteAsync(id);
        }

        public IQueryable<SellerDBModel> GetQuery()
        {
            return _repository.GetQuery();
        }

        public async Task<IEnumerable<SellerDBModel>> GetFromConditionAsync(Expression<Func<SellerDBModel, bool>> condition)
        {
            return await _repository.GetFromConditionAsync(condition);
        }

        public async Task<IEnumerable<SellerDBModel>> ProcessQueryAsync(IQueryable<SellerDBModel> query)
        {
            return await _repository.ProcessQueryAsync(query);
        }
    }
}
