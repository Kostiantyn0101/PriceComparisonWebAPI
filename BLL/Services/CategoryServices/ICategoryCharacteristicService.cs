using System.Linq.Expressions;
using Domain.Models.DBModels;
using Domain.Models.Request.Categories;
using Domain.Models.Response;

namespace BLL.Services.CategoryCharacteristicService
{
    public interface ICategoryCharacteristicService
    {
        Task<OperationDetailsResponseModel> CreateAsync(CategoryCharacteristicDBModel model);
        Task<List<OperationDetailsResponseModel>> CreateMultipleAsync(CategoryCharacteristicRequestModel request);
        Task<OperationDetailsResponseModel> UpdateAsync(CategoryCharacteristicDBModel entity);
        Task<OperationDetailsResponseModel> DeleteAsync(int categoryId, int characteristicId);
        Task<List<OperationDetailsResponseModel>> DeleteMultipleAsync(CategoryCharacteristicRequestModel request);
        IQueryable<CategoryCharacteristicDBModel> GetQuery();
        Task<IEnumerable<CategoryCharacteristicDBModel>> GetFromConditionAsync(Expression<Func<CategoryCharacteristicDBModel, bool>> condition);
        Task<IEnumerable<CategoryCharacteristicDBModel>> ProcessQueryAsync(IQueryable<CategoryCharacteristicDBModel> query);
    }
}
