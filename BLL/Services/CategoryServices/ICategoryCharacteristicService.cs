using System.Linq.Expressions;
using Domain.Models.DBModels;
using Domain.Models.Response;

namespace BLL.Services.CategoryService
{
    public interface ICategoryCharacteristicService
    {
        Task<OperationDetailsResponseModel> CreateAsync(CategoryCharacteristicDBModel model);
        Task<OperationDetailsResponseModel> UpdateAsync(CategoryCharacteristicDBModel entity);
        Task<OperationDetailsResponseModel> DeleteAsync(int categoryId, int characteristicId);
        IQueryable<CategoryCharacteristicDBModel> GetQuery();
        Task<IEnumerable<CategoryCharacteristicDBModel>> GetFromConditionAsync(Expression<Func<CategoryCharacteristicDBModel, bool>> condition);
        Task<IEnumerable<CategoryCharacteristicDBModel>> ProcessQueryAsync(IQueryable<CategoryCharacteristicDBModel> query);
    }
}
