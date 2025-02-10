namespace Domain.Models.Request.Products
{
    public class ProductCharacteristicUpdateRequestModel
    {
        public int ProductId { get; set; }
        public List<ProductCharacteristicValueUpdateModel> Characteristics { get; set; }
    }
}
