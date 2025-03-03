namespace Domain.Models.Request.Products
{
    public class ProductSellerReferenceClickUpdateRequestModel
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int SellerId { get; set; }
        public string UserIp { get; set; }
        public DateTime ClickedAt { get; set; }
    }
}
