using Domain.Models.DBModels;
using Domain.Models.Response;
using System.Linq.Expressions;

namespace DLL.Repository
{
    //deprecated
    public interface ICategoryCharacteristicRepository
    {
        Task<OperationDetailsResponseModel> CreateAsync(CategoryCharacteristicDBModel entity);
        Task<OperationDetailsResponseModel> UpdateAsync(CategoryCharacteristicDBModel entityNew);
        Task<OperationDetailsResponseModel> DeleteAsync(int categoryId, int characteristicId);

        Task<IEnumerable<CategoryCharacteristicDBModel>> GetFromConditionAsync(Expression<Func<CategoryCharacteristicDBModel, bool>> condition);
        Task<IEnumerable<CategoryCharacteristicDBModel>> ProcessQueryAsync(IQueryable<CategoryCharacteristicDBModel> query);
        IQueryable<CategoryCharacteristicDBModel> GetQuery();
    }
}
