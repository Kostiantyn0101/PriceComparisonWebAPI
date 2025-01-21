using Domain.Models.Primitives;

namespace Domain.Models.DBModels
{
    public class FeedbackImageDBModel : EntityDBModel
    {
        public int FeedbackId { get; set; }
        public FeedbackDBModel Feedback { get; set; }

        public string ImageUrl { get; set; }
    }
}
