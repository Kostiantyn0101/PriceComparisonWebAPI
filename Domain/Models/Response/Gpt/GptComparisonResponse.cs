using Domain.Models.Response.Products;

namespace Domain.Models.Response.Gpt
{
    public class GptComparisonResponse
    {
        public IEnumerable<ProductCharacteristicGroupResponseModel>? ProductA { get; set; }
        public IEnumerable<ProductCharacteristicGroupResponseModel>? ProductB { get; set; }
        public string? Explanation { get; set; }
    }
}
