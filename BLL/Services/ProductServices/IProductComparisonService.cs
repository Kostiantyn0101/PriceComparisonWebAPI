using Domain.Models.Response.Gpt;

namespace BLL.Services.ProductServices
{
    public interface IProductComparisonService
    {
        Task<GptComparisonProductCharacteristicResponseModel> CompareProductsAsync(int productIdA, int productIdB);
    }
}
