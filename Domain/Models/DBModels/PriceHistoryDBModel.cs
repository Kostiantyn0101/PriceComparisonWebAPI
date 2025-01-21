using Domain.Models.Primitives;

namespace Domain.Models.DBModels
{
    public class PriceHistoryDBModel : EntityDBModel
    {
        public int ProductId { get; set; }
        public ProductDBModel Product { get; set; }

        public int SellerId { get; set; }
        public SellerDBModel Seller { get; set; }

        public decimal PriceValue { get; set; }
        public DateTime CreatedAt { get; set; }
    }

}
