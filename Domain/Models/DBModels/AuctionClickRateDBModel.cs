using Domain.Models.Primitives;

namespace Domain.Models.DBModels
{
    public class AuctionClickRateDBModel : IEntity<int>
    {
        public int Id { get; set; }
        public int CategoryId { get; set; }
        public CategoryDBModel Category { get; set; }

        public int SellerId { get; set; }
        public SellerDBModel Seller { get; set; }

        public decimal ClickRate { get; set; }
    }
}
