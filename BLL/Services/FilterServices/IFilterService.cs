using Domain.Models.DBModels;
using Domain.Models.Request.Filters;
using Domain.Models.Response;
using Domain.Models.Response.Filters;
using System.Linq.Expressions;

namespace BLL.Services.FilterServices
{
    public interface IFilterService
    {
        Task<OperationResultModel<FilterDBModel>> CreateAsync(FilterCreateRequestModel request);
        Task<OperationResultModel<FilterDBModel>> UpdateAsync(FilterUpdateRequestModel request);
        Task<OperationResultModel<bool>> DeleteAsync(int id);
        IQueryable<FilterDBModel> GetQuery();
        Task<IEnumerable<FilterResponseModel>> GetFromConditionAsync(Expression<Func<FilterDBModel, bool>> condition);
        Task<IEnumerable<FilterDBModel>> ProcessQueryAsync(IQueryable<FilterDBModel> query);
    }
}
