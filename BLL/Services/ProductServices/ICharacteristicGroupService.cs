using Domain.Models.DBModels;
using Domain.Models.Request.Products;
using Domain.Models.Response;
using Domain.Models.Response.Products;
using System.Linq.Expressions;

namespace BLL.Services.ProductServices
{
    public interface ICharacteristicGroupService
    {
        Task<OperationResultModel<CharacteristicGroupDBModel>> CreateAsync(CharacteristicGroupCreateRequestModel request);
        Task<OperationResultModel<CharacteristicGroupDBModel>> UpdateAsync(CharacteristicGroupRequestModel request);
        Task<OperationResultModel<bool>> DeleteAsync(int id);
        IQueryable<CharacteristicGroupDBModel> GetQuery();
        Task<IEnumerable<CharacteristicGroupResponseModel>> GetFromConditionAsync(Expression<Func<CharacteristicGroupDBModel, bool>> condition);
        Task<IEnumerable<CharacteristicGroupDBModel>> ProcessQueryAsync(IQueryable<CharacteristicGroupDBModel> query);
    }
}
