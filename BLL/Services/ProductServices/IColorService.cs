using System.Linq.Expressions;
using Domain.Models.DBModels;
using Domain.Models.Request.Products;
using Domain.Models.Response;
using Domain.Models.Response.Products;

namespace BLL.Services.ProductServices
{
    public interface IColorService
    {
        Task<OperationResultModel<ColorDBModel>> CreateAsync(ColorCreateRequestModel request);
        Task<OperationResultModel<ColorDBModel>> UpdateAsync(ColorUpdateRequestModel request);
        Task<OperationResultModel<bool>> DeleteAsync(int id);
        IQueryable<ColorDBModel> GetQuery();
        Task<IEnumerable<ColorResponseModel>> GetFromConditionAsync(Expression<Func<ColorDBModel, bool>> condition);
        Task<IEnumerable<ColorDBModel>> ProcessQueryAsync(IQueryable<ColorDBModel> query);
    }
}
