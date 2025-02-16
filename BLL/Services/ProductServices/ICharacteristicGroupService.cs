using System.Linq.Expressions;
using Domain.Models.DBModels;
using Domain.Models.Request.Products;
using Domain.Models.Response;
using Domain.Models.Response.Products;

namespace BLL.Services.ProductServices
{
    public interface ICharacteristicGroupService
    {
        Task<OperationResultModel<bool>> CreateAsync(CharacteristicGroupRequestModel request);
        Task<OperationResultModel<bool>> UpdateAsync(CharacteristicGroupRequestModel request);
        Task<OperationResultModel<bool>> DeleteAsync(int id);
        IQueryable<CharacteristicGroupDBModel> GetQuery();
        Task<IEnumerable<CharacteristicGroupResponseModel>> GetFromConditionAsync(Expression<Func<CharacteristicGroupDBModel, bool>> condition);
        Task<IEnumerable<CharacteristicGroupDBModel>> ProcessQueryAsync(IQueryable<CharacteristicGroupDBModel> query);
    }
}
