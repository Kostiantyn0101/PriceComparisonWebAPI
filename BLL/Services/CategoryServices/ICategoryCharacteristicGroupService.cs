using System.Linq.Expressions;
using Domain.Models.DBModels;
using Domain.Models.Request.Categories;
using Domain.Models.Response.Categories;
using Domain.Models.Response;

namespace BLL.Services.CategoryServices
{
    public interface ICategoryCharacteristicGroupService
    {
        Task<OperationResultModel<CategoryCharacteristicGroupDBModel>> CreateAsync(CategoryCharacteristicGroupCreateRequestModel request);
        Task<OperationResultModel<CategoryCharacteristicGroupDBModel>> UpdateAsync(CategoryCharacteristicGroupRequestModel request);
        Task<OperationResultModel<bool>> DeleteAsync(int id);
        IQueryable<CategoryCharacteristicGroupDBModel> GetQuery();
        Task<IEnumerable<CategoryCharacteristicGroupResponseModel>> GetFromConditionAsync(Expression<Func<CategoryCharacteristicGroupDBModel, bool>> condition);
        Task<IEnumerable<CategoryCharacteristicGroupDBModel>> ProcessQueryAsync(IQueryable<CategoryCharacteristicGroupDBModel> query);
    }

}
