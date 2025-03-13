namespace Domain.Models.Request.Products
{
    public class ProductSellerReferenceClickStaisticRequestModel
    {
        public int SellerId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
