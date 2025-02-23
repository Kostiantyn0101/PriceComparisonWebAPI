using Domain.Models.Primitives;

namespace Domain.Models.DBModels
{
    public class ProductDBModel : EntityDBModel
    {
        public string Brand { get; set; }
        public string Title { get; set; }
        public string? ModelNumber { get; set; }
        public string? Description { get; set; }
        public string? GTIN { get; set; }
        public string? UPC { get; set; }

        public int CategoryId { get; set; }
        public CategoryDBModel Category { get; set; }

        public ICollection<ProductImageDBModel> ProductImages { get; set; }
        public ICollection<ProductVideoDBModel> ProductVideos { get; set; }
        public ICollection<FeedbackDBModel> Feedbacks { get; set; }
        public ICollection<ReviewDBModel> Reviews { get; set; }
        public ICollection<InstructionDBModel> Instructions { get; set; }
        public ICollection<ProductCharacteristicDBModel> ProductCharacteristics { get; set; }
        public ICollection<PriceDBModel> Prices { get; set; }
        public ICollection<PriceHistoryDBModel> PricesHistory { get; set; }
        public ICollection<ProductSellerLinkDBModel> SellerLinks { get; set; }
        public ICollection<ProductClicksDBModel> ProductClicks { get; set; }
    }
}
