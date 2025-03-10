using Domain.Models.Response;
using Domain.Models.Response.Filters;
using Domain.Models.Response.Products;

namespace BLL.Services.FilterServices
{
    public interface IProductFilterByCharacteristicService
    {
        Task<OperationResultModel<IEnumerable<FilterResponseModel>>> GetFiltersByProductIdAsync(int productId);
        Task<OperationResultModel<IEnumerable<ProductResponseModel>>> GetProductsByFilterIdAsync(int filterId);
        Task<OperationResultModel<IEnumerable<ProductResponseModel>>> GetProductsByFilterIdsAsync(int[] filterIds);
        Task<OperationResultModel<IEnumerable<FilterResponseModel>>> GetFiltersByCategoryIdAsync(int categoryId);
    }
}
