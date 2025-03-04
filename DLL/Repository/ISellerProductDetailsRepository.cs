using Domain.Models.DBModels;
using Domain.Models.Response;
using System.Linq.Expressions;

namespace DLL.Repository
{
    //deprecated
    public interface ISellerProductDetailsRepository
    {
        Task<OperationDetailsResponseModel> CreateAsync(SellerProductDetailsDBModel entity);
        Task<OperationDetailsResponseModel> UpdateAsync(SellerProductDetailsDBModel entityNew);
        Task<OperationDetailsResponseModel> DeleteAsync(int productId, int sellerId);

        Task<IEnumerable<SellerProductDetailsDBModel>> GetFromConditionAsync(Expression<Func<SellerProductDetailsDBModel, bool>> condition);
        Task<IEnumerable<SellerProductDetailsDBModel>> ProcessQueryAsync(IQueryable<SellerProductDetailsDBModel> query);
        IQueryable<SellerProductDetailsDBModel> GetQuery();
    }
}
