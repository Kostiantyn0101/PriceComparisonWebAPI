using System.Linq.Expressions;
using Domain.Models.DBModels;
using Domain.Models.Request.Products;
using Domain.Models.Response;
using Domain.Models.Response.Products;

namespace BLL.Services.ProductServices
{
    public interface ICharacteristicService
    {
        Task<OperationResultModel<bool>> CreateAsync(CharacteristicCreateRequestModel request);
        Task<OperationResultModel<bool>> UpdateAsync(CharacteristicRequestModel request);
        Task<OperationResultModel<bool>> DeleteAsync(int id);
        IQueryable<CharacteristicDBModel> GetQuery();
        Task<IEnumerable<CharacteristicResponseModel>> GetFromConditionAsync(Expression<Func<CharacteristicDBModel, bool>> condition);
        Task<IEnumerable<CharacteristicDBModel>> ProcessQueryAsync(IQueryable<CharacteristicDBModel> query);
    }
}
