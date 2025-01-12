using System.ComponentModel.DataAnnotations;

namespace Domain.Models.DBModels
{
    public class FeedbackDBModel
    {
        public int Id { get; set; }

        [Required]
        public int UserId { get; set; }
        public virtual UserDBModel User { get; set; }

        [Required]
        public int ProductId { get; set; }
        public virtual ProductDBModel Product { get; set; }

        [Required]
        public string FeedbackText { get; set; }

        [Required]
        public DateTime CreatedAt { get; set; }

        [Required]
        [Range(1, 5)]
        public int Rating { get; set; }

        public virtual ICollection<FeedbackImageDBModel> FeedbackImages { get; set; }
    }

}
