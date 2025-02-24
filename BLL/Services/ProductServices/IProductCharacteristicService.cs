using System.Linq.Expressions;
using Domain.Models.DBModels;
using Domain.Models.Request.Products;
using Domain.Models.Response;
using Domain.Models.Response.Products;

namespace BLL.Services.ProductServices
{
    public interface IProductCharacteristicService
    {
        Task<OperationResultModel<bool>> UpdateProductCharacteristicAsync(ProductCharacteristicUpdateRequestModel model);
        Task<IEnumerable<ProductCharacteristicResponseModel>> GetWithIncludeFromConditionAsync(Expression<Func<ProductCharacteristicDBModel, bool>> condition);
        Task<IEnumerable<ProductCharacteristicGroupResponseModel>> GetDetailedCharacteristics(int productId);
        Task<IEnumerable<ProductCharacteristicGroupResponseModel>> GetShortCharacteristics(int productId);

        Task<OperationResultModel<bool>> CreateAsync(ProductCharacteristicDBModel model);
        Task<OperationResultModel<bool>> UpdateAsync(ProductCharacteristicDBModel entity);
        Task<OperationDetailsResponseModel> DeleteAsync(int productId, int characteristicId);
        IQueryable<ProductCharacteristicDBModel> GetQuery();
        Task<IEnumerable<ProductCharacteristicResponseModel>> GetFromConditionAsync(Expression<Func<ProductCharacteristicDBModel, bool>> condition);
        Task<IEnumerable<ProductCharacteristicDBModel>> ProcessQueryAsync(IQueryable<ProductCharacteristicDBModel> query);
    }
}
