namespace Domain.Models.Request.Feedback
{
    public class FeedbackCreateRequestModel
    {
        public int UserId { get; set; }
        public int ProductId { get; set; }
        public string FeedbackText { get; set; }
        public int Rating { get; set; }
    }
}
