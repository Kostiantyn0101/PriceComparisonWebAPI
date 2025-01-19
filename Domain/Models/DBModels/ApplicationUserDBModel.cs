using Microsoft.AspNetCore.Identity;

namespace Domain.Models.DBModels
{
    public class ApplicationUserDBModel : IdentityUser<int>
    {
        public DateTime DateCreated { get; set; }
        public string Provider { get; set; } = null!;
        public string? RefreshToken { get; set; }
        public DateTimeOffset RefreshTokenExpiryTime { get; set; }


        public ICollection<SellerDBModel> Sellers { get; set; }
        public ICollection<FeedbackDBModel> Feedbacks { get; set; }
    }

}
