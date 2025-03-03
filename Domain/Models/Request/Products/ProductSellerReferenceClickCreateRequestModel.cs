namespace Domain.Models.Request.Products
{
    public class ProductSellerReferenceClickCreateRequestModel
    {
        public int ProductId { get; set; }
        public int SellerId { get; set; }
        public string UserIp { get; set; }
    }
}
