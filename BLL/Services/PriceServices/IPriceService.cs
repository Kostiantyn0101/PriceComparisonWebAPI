using System.Linq.Expressions;
using Domain.Models.DBModels;
using Domain.Models.Response;

namespace BLL.Services.PriceServices
{
    public interface IPriceService
    {
        Task<OperationResultModel<SellerProductDetailsDBModel>> CreateAsync(SellerProductDetailsDBModel model);
        Task<OperationDetailsResponseModel> UpdateAsync(SellerProductDetailsDBModel entity);
        Task<OperationDetailsResponseModel> DeleteAsync(int productId, int sellerId);
        IQueryable<SellerProductDetailsDBModel> GetQuery();
        Task<IEnumerable<SellerProductDetailsDBModel>> GetFromConditionAsync(Expression<Func<SellerProductDetailsDBModel, bool>> condition);
        Task<IEnumerable<SellerProductDetailsDBModel>> ProcessQueryAsync(IQueryable<SellerProductDetailsDBModel> query);
    }
}
