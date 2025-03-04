using Domain.Models.DBModels;
using Domain.Models.Response;
using System.Linq.Expressions;

namespace DLL.Repository
{
    //deprecated
    public interface IProductCharacteristicRepository
    {
        Task<OperationDetailsResponseModel> CreateAsync(ProductCharacteristicDBModel entity);
        Task<OperationDetailsResponseModel> UpdateAsync(ProductCharacteristicDBModel entityNew);
        Task<OperationDetailsResponseModel> DeleteAsync(int productId, int characteristicId);

        Task<IEnumerable<ProductCharacteristicDBModel>> GetFromConditionAsync(Expression<Func<ProductCharacteristicDBModel, bool>> condition);
        Task<IEnumerable<ProductCharacteristicDBModel>> ProcessQueryAsync(IQueryable<ProductCharacteristicDBModel> query);
        IQueryable<ProductCharacteristicDBModel> GetQuery();
    }
}
