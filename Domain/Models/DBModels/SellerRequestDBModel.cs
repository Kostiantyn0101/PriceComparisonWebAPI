using Domain.Models.Primitives;

namespace Domain.Models.DBModels
{
    public class SellerRequestDBModel : IEntity<int>
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string StoreName { get; set; }
        public string WebsiteUrl { get; set; }
        public string ContactPerson { get; set; }
        public string ContactPhone { get; set; }
        public string? StoreComment { get; set; }
        public bool IsProcessed { get; set; }
        public bool IsApproved { get; set; }
        public string? RefusalReason { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? ProcessedAt { get; set; }

        public ApplicationUserDBModel User { get; set; }
    }
}
