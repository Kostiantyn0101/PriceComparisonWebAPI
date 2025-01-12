using System.ComponentModel.DataAnnotations;

namespace Domain.Models.DBModels
{
    public class ProductDBModel
    {
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        public string? Description { get; set; }

        [Required]
        public int CategoryId { get; set; }
        public virtual CategoryDBModel Category { get; set; }

        public virtual ICollection<ProductImageDBModel> ProductImages { get; set; }
        public virtual ICollection<ProductVideoDBModel> ProductVideos { get; set; }
        public virtual ICollection<FeedbackDBModel> Feedbacks { get; set; }
        public virtual ICollection<ReviewDBModel> Reviews { get; set; }
        public virtual ICollection<InstructionDBModel> Instructions { get; set; }
        public virtual ICollection<ProductCharacteristicDBModel> ProductCharacteristics { get; set; }
        public virtual ICollection<PriceDBModel> Prices { get; set; }
        public virtual ICollection<PriceHistoryDBModel> PricesHistory { get; set; }
    }
}
