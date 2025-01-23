using Domain.Models.Primitives;

namespace Domain.Models.DBModels
{
    public class ClickTrackingDBModel : EntityDBModel
    {
        public int ProductId { get; set; }
        public ProductDBModel Product { get; set; }

        public int SellerId { get; set; }
        public SellerDBModel Seller { get; set; }

        public int ProductSellerLinkId { get; set; }
        public ProductSellerLinkDBModel ProductSellerLink { get; set; }

        public DateTime ClickedAt { get; set; }
        public string UserIp { get; set; }
    }
}
