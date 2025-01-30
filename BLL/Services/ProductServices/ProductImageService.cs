using System.Linq.Expressions;
using DLL.Repository;
using Domain.Models.DBModels;
using Domain.Models.Response;

namespace BLL.Services.ProductService
{
    public class ProductImageService : IProductImageService
    {
        private readonly IRepository<ProductImageDBModel> _repository;

        public ProductImageService(IRepository<ProductImageDBModel> repository)
        {
            _repository = repository;
        }

        public async Task<OperationDetailsResponseModel> CreateAsync(ProductImageDBModel model)
        {
            return await _repository.CreateAsync(model);
        }

        public async Task<OperationDetailsResponseModel> UpdateAsync(ProductImageDBModel entity)
        {
            return await _repository.UpdateAsync(entity);
        }

        public async Task<OperationDetailsResponseModel> DeleteAsync(int id)
        {
            return await _repository.DeleteAsync(id);
        }

        public IQueryable<ProductImageDBModel> GetQuery()
        {
            return _repository.GetQuery();
        }

        public async Task<IEnumerable<ProductImageDBModel>> GetFromConditionAsync(Expression<Func<ProductImageDBModel, bool>> condition)
        {
            return await _repository.GetFromConditionAsync(condition);
        }

        public async Task<IEnumerable<ProductImageDBModel>> ProcessQueryAsync(IQueryable<ProductImageDBModel> query)
        {
            return await _repository.ProcessQueryAsync(query);
        }
    }
}
