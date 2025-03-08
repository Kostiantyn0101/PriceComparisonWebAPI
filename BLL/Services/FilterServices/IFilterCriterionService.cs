using Domain.Models.DBModels;
using Domain.Models.Request.Filters;
using Domain.Models.Response;
using Domain.Models.Response.Filters;
using System.Linq.Expressions;

namespace BLL.Services.FilterServices
{
    public interface IFilterCriterionService
    {
        Task<OperationResultModel<FilterCriterionDBModel>> CreateAsync(FilterCriterionCreateRequestModel request);
        Task<OperationResultModel<FilterCriterionDBModel>> UpdateAsync(FilterCriterionUpdateRequestModel request);
        Task<OperationResultModel<bool>> DeleteAsync(int id);
        IQueryable<FilterCriterionDBModel> GetQuery();
        Task<IEnumerable<FilterCriterionResponseModel>> GetFromConditionAsync(Expression<Func<FilterCriterionDBModel, bool>> condition);
        Task<IEnumerable<FilterCriterionDBModel>> ProcessQueryAsync(IQueryable<FilterCriterionDBModel> query);
    }
}
