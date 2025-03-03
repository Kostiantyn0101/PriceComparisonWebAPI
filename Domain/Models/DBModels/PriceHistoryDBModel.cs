using Domain.Models.Primitives;

namespace Domain.Models.DBModels
{
    public class PriceHistoryDBModel : IEntity<int>
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public ProductDBModel Product { get; set; }

        public int SellerId { get; set; }
        public SellerDBModel Seller { get; set; }

        public decimal PriceValue { get; set; }
        public DateTime PriceDate { get; set; }
        public DateTime CreatedAt { get; set; }
    }

}
