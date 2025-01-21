using Domain.Models.DBModels;
using Domain.Models.Response;
using System.Linq.Expressions;

namespace DLL.Repository
{
    public interface IPriceRepository
    {
        Task<OperationDetailsResponseModel> CreateAsync(PriceDBModel entity);
        Task<OperationDetailsResponseModel> UpdateAsync(PriceDBModel entityNew);
        Task<OperationDetailsResponseModel> DeleteAsync(int productId, int sellerId);

        Task<IEnumerable<PriceDBModel>> GetFromConditionAsync(Expression<Func<PriceDBModel, bool>> condition);
        Task<IEnumerable<PriceDBModel>> ProcessQueryAsync(IQueryable<PriceDBModel> query);
        IQueryable<PriceDBModel> GetQuery();
    }
}
