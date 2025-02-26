using Domain.Models.Primitives;

namespace Domain.Models.DBModels
{
    public class SellerProductDetailsDBModel
    {
        public int ProductId { get; set; }
        public ProductDBModel Product { get; set; }

        public int SellerId { get; set; }
        public SellerDBModel Seller { get; set; }

        public decimal PriceValue { get; set; }
        public DateTime LastUpdated { get; set; }
        public string ProductStoreUrl { get; set; }
    }
}
