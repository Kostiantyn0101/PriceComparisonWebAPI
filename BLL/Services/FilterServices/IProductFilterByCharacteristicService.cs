using Domain.Models.Response.Filters;
using Domain.Models.Response.Products;

namespace BLL.Services.FilterServices
{
    public interface IProductFilterByCharacteristicService
    {
        Task<IEnumerable<FilterResponseModel>> GetFiltersByProductIdAsync(int productId);
        Task<IEnumerable<ProductResponseModel>> GetProductsByFilterIdAsync(int filterId);
    }
}
