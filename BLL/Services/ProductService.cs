using System.Linq.Expressions;
using DLL.Repository;
using Domain.Models.DBModels;
using Domain.Models.Response;

namespace BLL.Services
{
    public class ProductService : IProductService
    {
        private readonly IRepository<ProductDBModel> _repository;

        public ProductService(IRepository<ProductDBModel> repository)
        {
            _repository = repository;
        }

        public async Task<OperationDetailsResponseModel> CreateAsync(ProductDBModel model)
        {
            return await _repository.CreateAsync(model);
        }

        public async Task<OperationDetailsResponseModel> UpdateAsync(ProductDBModel entity)
        {
            return await _repository.UpdateAsync(entity);
        }

        public async Task<OperationDetailsResponseModel> DeleteAsync(int id)
        {
            return await _repository.DeleteAsync(id);
        }

        public IQueryable<ProductDBModel> GetQuery()
        {
            return _repository.GetQuery();
        }

        public async Task<IEnumerable<ProductDBModel>> GetFromConditionAsync(Expression<Func<ProductDBModel, bool>> condition)
        {
            return await _repository.GetFromConditionAsync(condition);
        }

        public async Task<IEnumerable<ProductDBModel>> ProcessQueryAsync(IQueryable<ProductDBModel> query)
        {
            return await _repository.ProcessQueryAsync(query);
        }
    }
}
