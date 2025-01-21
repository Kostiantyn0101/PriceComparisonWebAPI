using System.Linq.Expressions;
using Domain.Models.DBModels;
using Domain.Models.Response;

namespace BLL.Services
{
    public interface IPriceHistoryService
    {
        Task<OperationDetailsResponseModel> CreateAsync(PriceHistoryDBModel model);
        Task<OperationDetailsResponseModel> UpdateAsync(PriceHistoryDBModel entity);
        Task<OperationDetailsResponseModel> DeleteAsync(int id);
        IQueryable<PriceHistoryDBModel> GetQuery();
        Task<IEnumerable<PriceHistoryDBModel>> GetFromConditionAsync(Expression<Func<PriceHistoryDBModel, bool>> condition);
        Task<IEnumerable<PriceHistoryDBModel>> ProcessQueryAsync(IQueryable<PriceHistoryDBModel> query);
    }
}
