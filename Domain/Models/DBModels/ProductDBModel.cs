using Domain.Models.Primitives;

namespace Domain.Models.DBModels
{
    public class ProductDBModel : EntityDBModel
    {
        private string _title;

        public string Brand { get; set; }
        public string Title
        {
            get => _title;
            set
            {
                _title = value;
                NormalizedTitle = value?.Trim().ToUpperInvariant(); ;
            }
        }
        public string NormalizedTitle { get; private set; }
        public string? ModelNumber { get; set; }
        public string? Description { get; set; }
        public string? GTIN { get; set; }
        public string? UPC { get; set; }
        public bool IsUnderModeration { get; set; }
        public DateTime AddedToDatabase { get; set; }

        public int CategoryId { get; set; }
        public CategoryDBModel Category { get; set; }

        public ICollection<ProductImageDBModel> ProductImages { get; set; }
        public ICollection<ProductVideoDBModel> ProductVideos { get; set; }
        public ICollection<FeedbackDBModel> Feedbacks { get; set; }
        public ICollection<ReviewDBModel> Reviews { get; set; }
        public ICollection<InstructionDBModel> Instructions { get; set; }
        public ICollection<ProductCharacteristicDBModel> ProductCharacteristics { get; set; }
        public ICollection<SellerProductDetailsDBModel> SellerProductDetails { get; set; }
        public ICollection<PriceHistoryDBModel> PricesHistories { get; set; }
        public ICollection<ProductClicksDBModel> ProductClicks { get; set; }
        public ICollection<ProductReferenceClickDBModel> ProductSellerReferenceClicks { get; set; }
    }
}
