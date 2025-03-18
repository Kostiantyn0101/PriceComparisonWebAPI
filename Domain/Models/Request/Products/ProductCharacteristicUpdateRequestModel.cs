namespace Domain.Models.Request.Products
{
    public class ProductCharacteristicUpdateRequestModel
    {
        public int? ProductId { get; set; }
        public int? BaseProductId { get; set; }
        public List<ProductCharacteristicUpdateModel> Characteristics { get; set; }
    }
}
