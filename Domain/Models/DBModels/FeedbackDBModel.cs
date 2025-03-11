using Domain.Models.Primitives;

namespace Domain.Models.DBModels
{
    public class FeedbackDBModel : IEntity<int>
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int BaseProductId { get; set; }
        public string FeedbackText { get; set; }
        public DateTime CreatedAt { get; set; }
        public int Rating { get; set; }

        public ApplicationUserDBModel User { get; set; }
        public BaseProductDBModel BaseProduct { get; set; }
        
        public ICollection<FeedbackImageDBModel> FeedbackImages { get; set; }
    }
}
