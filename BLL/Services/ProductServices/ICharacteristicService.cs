using Domain.Models.DBModels;
using Domain.Models.Request.Products;
using Domain.Models.Response;
using Domain.Models.Response.Products;
using System.Linq.Expressions;

namespace BLL.Services.ProductServices
{
    public interface ICharacteristicService
    {
        Task<OperationResultModel<CharacteristicResponseModel>> CreateAsync(CharacteristicCreateRequestModel request);
        Task<OperationResultModel<CharacteristicDBModel>> UpdateAsync(CharacteristicRequestModel request);
        Task<OperationResultModel<bool>> DeleteAsync(int id);
        IQueryable<CharacteristicDBModel> GetQuery();
        Task<IEnumerable<CharacteristicResponseModel>> GetFromConditionAsync(Expression<Func<CharacteristicDBModel, bool>> condition);
        Task<IEnumerable<CharacteristicDBModel>> ProcessQueryAsync(IQueryable<CharacteristicDBModel> query);
    }
}
