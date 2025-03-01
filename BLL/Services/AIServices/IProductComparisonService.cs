using Domain.Models.Response.Gpt;

namespace BLL.Services.AIServices
{
    public interface IProductComparisonService
    {
        Task<AIComparisonProductCharacteristicResponseModel> CompareProductsAsync(int productIdA, int productIdB, AIProvider? provider = null);
    }
}
