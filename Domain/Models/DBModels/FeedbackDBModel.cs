namespace Domain.Models.DBModels
{
    public class FeedbackDBModel
    {
        public int Id { get; set; }

        public int UserId { get; set; }
        public UserDBModel User { get; set; }

        public int ProductId { get; set; }
        public ProductDBModel Product { get; set; }

        public string FeedbackText { get; set; }
        public DateTime CreatedAt { get; set; }
        public int Rating { get; set; }

        public ICollection<FeedbackImageDBModel> FeedbackImages { get; set; }
    }

}
