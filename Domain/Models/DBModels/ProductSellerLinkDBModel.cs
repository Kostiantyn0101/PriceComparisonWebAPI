using Domain.Models.Primitives;

namespace Domain.Models.DBModels
{
    public class ProductSellerLinkDBModel : EntityDBModel
    {
        public int ProductId { get; set; }
        public ProductDBModel Product { get; set; }

        public string SellerUrl { get; set; }
        public ICollection<ClickTrackingDBModel> ClickTrackings { get; set; }

    }
}
