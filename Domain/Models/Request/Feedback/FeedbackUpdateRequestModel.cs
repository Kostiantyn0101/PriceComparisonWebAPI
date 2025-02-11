namespace Domain.Models.Request.Feedback
{
    public class FeedbackUpdateRequestModel
    {
        public int Id { get; set; }
        public string FeedbackText { get; set; }
        public int Rating { get; set; }
    }
}
