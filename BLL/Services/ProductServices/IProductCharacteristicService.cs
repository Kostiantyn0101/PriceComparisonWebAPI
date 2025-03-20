using Domain.Models.DBModels;
using Domain.Models.Request.Products;
using Domain.Models.Response;
using Domain.Models.Response.Products;
using System.Linq.Expressions;

namespace BLL.Services.ProductServices
{
    public interface IProductCharacteristicService
    {
        Task<OperationResultModel<IEnumerable<ProductCharacteristicResponseModel>>> CreateProductCharacteristicsAsync(IEnumerable<ProductCharacteristicCreateRequestModel> models);
        Task<OperationResultModel<IEnumerable<ProductCharacteristicResponseModel>>> UpdateProductCharacteristicsAsync(IEnumerable<ProductCharacteristicUpdateRequestModel> models);
        Task<IEnumerable<ProductCharacteristicResponseModel>> GetProductCharacteristicsAsync(int productId);
        Task<IEnumerable<ProductCharacteristicResponseModel>> GetBaseProductCharacteristicsAsync(int baseProductId);
        Task<IEnumerable<ProductCharacteristicGroupResponseModel>> GetGroupedProductCharacteristicsAsync(int productId);
        Task<IEnumerable<ProductCharacteristicGroupResponseModel>> GetShortGroupedProductCharacteristicsAsync(int productId);

        Task<OperationResultModel<ProductCharacteristicResponseModel>> CreateAsync(ProductCharacteristicCreateRequestModel model);
        Task<OperationResultModel<ProductCharacteristicResponseModel>> UpdateAsync(ProductCharacteristicUpdateRequestModel entity);
        Task<OperationResultModel<bool>> DeleteAsync(int id);
        IQueryable<ProductCharacteristicDBModel> GetQuery();
        Task<IEnumerable<ProductCharacteristicResponseModel>> GetFromConditionAsync(Expression<Func<ProductCharacteristicDBModel, bool>> condition);
        Task<IEnumerable<ProductCharacteristicDBModel>> ProcessQueryAsync(IQueryable<ProductCharacteristicDBModel> query);
    }
}
