using System.Linq.Expressions;
using Domain.Models.DBModels;
using Domain.Models.Response;

namespace BLL.Services.PriceServices
{
    public interface IPriceHistoryService
    {
        Task<OperationResultModel<PriceHistoryDBModel>> CreateAsync(PriceHistoryDBModel model);
        Task<OperationResultModel<PriceHistoryDBModel>> UpdateAsync(PriceHistoryDBModel entity);
        Task<OperationResultModel<bool>> DeleteAsync(int id);
        IQueryable<PriceHistoryDBModel> GetQuery();
        Task<IEnumerable<PriceHistoryDBModel>> GetFromConditionAsync(Expression<Func<PriceHistoryDBModel, bool>> condition);
        Task<IEnumerable<PriceHistoryDBModel>> ProcessQueryAsync(IQueryable<PriceHistoryDBModel> query);
    }
}
