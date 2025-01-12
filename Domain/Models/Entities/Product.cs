using System.ComponentModel.DataAnnotations;


namespace Domain.Models.Entities
{
    public class Product
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(255)]
        public string Title { get; set; }

        public string Description { get; set; }

        [Required]
        public int CategoryId { get; set; }
        public virtual Category Category { get; set; }

        public virtual ICollection<ProductImage> ProductImages { get; set; }
        public virtual ICollection<ProductVideo> ProductVideos { get; set; }
        public virtual ICollection<Feedback> Feedbacks { get; set; }
        public virtual ICollection<Review> Reviews { get; set; }
        public virtual ICollection<Instruction> Instructions { get; set; }
        public virtual ICollection<ProductCharacteristic> ProductCharacteristics { get; set; }
        public virtual ICollection<Price> Prices { get; set; }
        public virtual ICollection<PriceHistory> PricesHistory { get; set; }
    }
}
