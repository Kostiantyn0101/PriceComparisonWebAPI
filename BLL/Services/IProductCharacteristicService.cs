using System.Linq.Expressions;
using Domain.Models.DBModels;
using Domain.Models.Response;

namespace BLL.Services
{
    public interface IProductCharacteristicService
    {
        Task<OperationDetailsResponseModel> CreateAsync(ProductCharacteristicDBModel model);
        Task<OperationDetailsResponseModel> UpdateAsync(ProductCharacteristicDBModel entity);
        Task<OperationDetailsResponseModel> DeleteAsync(int productId, int characteristicId);
        IQueryable<ProductCharacteristicDBModel> GetQuery();
        Task<IEnumerable<ProductCharacteristicDBModel>> GetFromConditionAsync(Expression<Func<ProductCharacteristicDBModel, bool>> condition);
        Task<IEnumerable<ProductCharacteristicDBModel>> ProcessQueryAsync(IQueryable<ProductCharacteristicDBModel> query);
    }
}
