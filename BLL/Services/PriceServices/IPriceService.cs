using System.Linq.Expressions;
using Domain.Models.DBModels;
using Domain.Models.Response;

namespace BLL.Services.PriceServices
{
    public interface IPriceService
    {
        Task<OperationDetailsResponseModel> CreateAsync(PriceDBModel model);
        Task<OperationDetailsResponseModel> UpdateAsync(PriceDBModel entity);
        Task<OperationDetailsResponseModel> DeleteAsync(int productId, int sellerId);
        IQueryable<PriceDBModel> GetQuery();
        Task<IEnumerable<PriceDBModel>> GetFromConditionAsync(Expression<Func<PriceDBModel, bool>> condition);
        Task<IEnumerable<PriceDBModel>> ProcessQueryAsync(IQueryable<PriceDBModel> query);
    }
}
