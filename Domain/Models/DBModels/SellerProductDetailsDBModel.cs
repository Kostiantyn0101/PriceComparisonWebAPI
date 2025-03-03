using Domain.Models.Primitives;

namespace Domain.Models.DBModels
{
    public class SellerProductDetailsDBModel : IEntity<CompositeKey<int, int>>
    {
        public CompositeKey<int, int> Id
        {
            get => new CompositeKey<int, int> { Key1 = this.ProductId, Key2 = this.SellerId };
            set
            {
                if (value != null)
                {
                    ProductId = value.Key1;
                    SellerId = value.Key2;
                }
            }
        }
        public int ProductId { get; set; }
        public ProductDBModel Product { get; set; }

        public int SellerId { get; set; }
        public SellerDBModel Seller { get; set; }

        public decimal PriceValue { get; set; }
        public DateTime LastUpdated { get; set; }
        public string ProductStoreUrl { get; set; }
    }
}
