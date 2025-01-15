namespace Domain.Models.DBModels
{
    public class FeedbackImageDBModel
    {
        public int Id { get; set; }

        public int FeedbackId { get; set; }
        public FeedbackDBModel Feedback { get; set; }

        public string ImageUrl { get; set; }
    }
}
