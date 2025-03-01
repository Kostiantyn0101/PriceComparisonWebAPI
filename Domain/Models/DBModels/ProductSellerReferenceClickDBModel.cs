using Domain.Models.Primitives;

namespace Domain.Models.DBModels
{
    public class ProductSellerReferenceClickDBModel : EntityDBModel
    {
        public int ProductId { get; set; }
        public ProductDBModel Product { get; set; }

        public int SellerId { get; set; }
        public SellerDBModel Seller { get; set; }

        public string UserIp { get; set; }
        public DateTime ClickedAt { get; set; }
    }
}
