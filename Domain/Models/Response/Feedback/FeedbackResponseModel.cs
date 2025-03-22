namespace Domain.Models.Response.Feedback
{
    public class FeedbackResponseModel
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
        public int BaseProductId { get; set; }
        public string FeedbackText { get; set; }
        public DateTime CreatedAt { get; set; }
        public int Rating { get; set; }
        public List<string> FeedbackImageUrls { get; set; }
    }
}
