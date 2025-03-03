using System.Linq.Expressions;
using DLL.Repository;
using Domain.Models.DBModels;
using Domain.Models.Primitives;
using Domain.Models.Response;

namespace BLL.Services.PriceServices
{
    public class PriceService : IPriceService
    {
        private readonly ICompositeKeyRepository<SellerProductDetailsDBModel, int, int> _repository;

        public PriceService(ICompositeKeyRepository<SellerProductDetailsDBModel, int, int> repository)
        {
            _repository = repository;
        }

        public async Task<OperationResultModel<SellerProductDetailsDBModel>> CreateAsync(SellerProductDetailsDBModel model)
        {

            return await _repository.CreateAsync(model);
        }

        public async Task<OperationDetailsResponseModel> UpdateAsync(SellerProductDetailsDBModel entity)
        {
            return await _repository.UpdateAsync(entity);
        }

        public async Task<OperationDetailsResponseModel> DeleteAsync(int productId, int sellerId)
        {
            var compositeKey = new CompositeKey<int, int> { Key1 = productId, Key2 = sellerId };
            return await _repository.DeleteAsync(compositeKey);
        }

        public IQueryable<SellerProductDetailsDBModel> GetQuery()
        {
            return _repository.GetQuery();
        }

        public async Task<IEnumerable<SellerProductDetailsDBModel>> GetFromConditionAsync(Expression<Func<SellerProductDetailsDBModel, bool>> condition)
        {
            return await _repository.GetFromConditionAsync(condition);
        }

        public async Task<IEnumerable<SellerProductDetailsDBModel>> ProcessQueryAsync(IQueryable<SellerProductDetailsDBModel> query)
        {
            return await _repository.ProcessQueryAsync(query);
        }
    }
}
