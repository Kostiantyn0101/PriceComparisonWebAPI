using System.Linq.Expressions;
using Domain.Models.DBModels;
using Domain.Models.Request.Categories;
using Domain.Models.Response;
using Domain.Models.Response.Categories;

namespace BLL.Services.CategoryCharacteristicService
{
    public interface ICategoryCharacteristicService
    {
        Task<OperationResultModel<bool>> CreateAsync(CategoryCharacteristicDBModel model);
        Task<OperationResultModel<bool>> CreateMultipleAsync(CategoryCharacteristicRequestModel request);
        Task<OperationDetailsResponseModel> UpdateAsync(CategoryCharacteristicDBModel entity);
        Task<OperationResultModel<bool>> DeleteAsync(int categoryId, int characteristicId);
        Task<OperationResultModel<bool>> DeleteMultipleAsync(CategoryCharacteristicRequestModel request);
        Task<OperationResultModel<IEnumerable<CategoryCharacteristicResponseModel>>> GetMappedCharacteristicsAsync(int categoryId);
        IQueryable<CategoryCharacteristicDBModel> GetQuery();
        Task<IEnumerable<CategoryCharacteristicDBModel>> GetFromConditionAsync(Expression<Func<CategoryCharacteristicDBModel, bool>> condition);
        Task<IEnumerable<CategoryCharacteristicDBModel>> ProcessQueryAsync(IQueryable<CategoryCharacteristicDBModel> query);
    }
}
