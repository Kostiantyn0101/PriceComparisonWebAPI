using System.ComponentModel.DataAnnotations;

namespace Domain.Models.Entities
{
    public class FeedbackImage
    {
        public int Id { get; set; }

        [Required]
        public int FeedbackId { get; set; }
        public virtual Feedback Feedback { get; set; }

        [Required]
        [MaxLength(2083)]
        public string ImageUrl { get; set; }
    }
}
