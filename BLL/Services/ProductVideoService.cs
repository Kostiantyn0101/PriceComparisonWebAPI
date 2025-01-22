using System.Linq.Expressions;
using DLL.Repository;
using Domain.Models.DBModels;
using Domain.Models.Response;

namespace BLL.Services
{
    public class ProductVideoService : IProductVideoService
    {
        private readonly IRepository<ProductVideoDBModel> _repository;

        public ProductVideoService(IRepository<ProductVideoDBModel> repository)
        {
            _repository = repository;
        }

        public async Task<OperationDetailsResponseModel> CreateAsync(ProductVideoDBModel model)
        {
            return await _repository.CreateAsync(model);
        }

        public async Task<OperationDetailsResponseModel> UpdateAsync(ProductVideoDBModel entity)
        {
            return await _repository.UpdateAsync(entity);
        }

        public async Task<OperationDetailsResponseModel> DeleteAsync(int id)
        {
            return await _repository.DeleteAsync(id);
        }

        public IQueryable<ProductVideoDBModel> GetQuery()
        {
            return _repository.GetQuery();
        }

        public async Task<IEnumerable<ProductVideoDBModel>> GetFromConditionAsync(Expression<Func<ProductVideoDBModel, bool>> condition)
        {
            return await _repository.GetFromConditionAsync(condition);
        }

        public async Task<IEnumerable<ProductVideoDBModel>> ProcessQueryAsync(IQueryable<ProductVideoDBModel> query)
        {
            return await _repository.ProcessQueryAsync(query);
        }
    }
}
