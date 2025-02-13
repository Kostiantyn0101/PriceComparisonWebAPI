using Domain.Models.Primitives;

namespace Domain.Models.DBModels
{
    public class FeedbackDBModel : EntityDBModel
    {
        public int UserId { get; set; }
        public ApplicationUserDBModel User { get; set; }

        public int ProductId { get; set; }
        public ProductDBModel Product { get; set; }

        public string FeedbackText { get; set; }
        public DateTime CreatedAt { get; set; }
        public int Rating { get; set; }

        public ICollection<FeedbackImageDBModel> FeedbackImages { get; set; }
    }
}
