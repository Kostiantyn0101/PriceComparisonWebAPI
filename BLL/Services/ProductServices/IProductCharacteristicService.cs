using Domain.Models.DBModels;
using Domain.Models.Request.Products;
using Domain.Models.Response;
using Domain.Models.Response.Products;
using System.Linq.Expressions;

namespace BLL.Services.ProductServices
{
    public interface IProductCharacteristicService
    {
        Task<OperationResultModel<bool>> UpdateProductCharacteristicAsync(ProductCharacteristicUpdateRequestModel model);
        Task<IEnumerable<ProductCharacteristicResponseModel>> GetWithIncludeFromConditionAsync(Expression<Func<ProductCharacteristicDBModel, bool>> condition);
        Task<IEnumerable<ProductCharacteristicGroupResponseModel>> GetDetailedCharacteristics(int productId);
        Task<IEnumerable<ProductCharacteristicGroupResponseModel>> GetShortCharacteristics(int productId);

        Task<OperationResultModel<ProductCharacteristicDBModel>> CreateAsync(ProductCharacteristicDBModel model);
        Task<OperationResultModel<ProductCharacteristicDBModel>> UpdateAsync(ProductCharacteristicDBModel entity);
        Task<OperationResultModel<bool>> DeleteAsync(int id);
        IQueryable<ProductCharacteristicDBModel> GetQuery();
        Task<IEnumerable<ProductCharacteristicResponseModel>> GetFromConditionAsync(Expression<Func<ProductCharacteristicDBModel, bool>> condition);
        Task<IEnumerable<ProductCharacteristicDBModel>> ProcessQueryAsync(IQueryable<ProductCharacteristicDBModel> query);
    }
}
