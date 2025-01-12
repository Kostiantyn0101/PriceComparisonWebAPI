using System.ComponentModel.DataAnnotations;

namespace Domain.Models.DBModels
{
    public class FeedbackImageDBModel
    {
        public int Id { get; set; }

        [Required]
        public int FeedbackId { get; set; }
        public virtual FeedbackDBModel Feedback { get; set; }

        [Required]
        public string ImageUrl { get; set; }
    }
}
