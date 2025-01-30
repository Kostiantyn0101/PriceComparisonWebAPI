using System.Linq.Expressions;
using DLL.Repository;
using Domain.Models.DBModels;
using Domain.Models.Response;

namespace BLL.Services.ProductService
{
    public class ProductSellerLinkService : IProductSellerLinkService
    {
        private readonly IRepository<ProductSellerLinkDBModel> _repository;

        public ProductSellerLinkService(IRepository<ProductSellerLinkDBModel> repository)
        {
            _repository = repository;
        }

        public async Task<OperationDetailsResponseModel> CreateAsync(ProductSellerLinkDBModel model)
        {
            return await _repository.CreateAsync(model);
        }

        public async Task<OperationDetailsResponseModel> UpdateAsync(ProductSellerLinkDBModel entity)
        {
            return await _repository.UpdateAsync(entity);
        }

        public async Task<OperationDetailsResponseModel> DeleteAsync(int id)
        {
            return await _repository.DeleteAsync(id);
        }

        public IQueryable<ProductSellerLinkDBModel> GetQuery()
        {
            return _repository.GetQuery();
        }

        public async Task<IEnumerable<ProductSellerLinkDBModel>> GetFromConditionAsync(Expression<Func<ProductSellerLinkDBModel, bool>> condition)
        {
            return await _repository.GetFromConditionAsync(condition);
        }

        public async Task<IEnumerable<ProductSellerLinkDBModel>> ProcessQueryAsync(IQueryable<ProductSellerLinkDBModel> query)
        {
            return await _repository.ProcessQueryAsync(query);
        }
    }
}
