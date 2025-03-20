namespace Domain.Models.Response.Seller
{
    public class SellerProductPricesResponseModel
    {
        public int ProductId { get; set; }  
        public decimal MinPriceValue { get; set; }  
        public decimal MaxPriceValue { get; set; }  
    }
}
