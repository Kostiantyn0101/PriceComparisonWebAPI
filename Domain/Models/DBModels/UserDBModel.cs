using System.ComponentModel.DataAnnotations;

namespace Domain.Models.DBModels
{
    public class UserDBModel
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public int RoleId { get; set; }
        public virtual RoleDBModel Role { get; set; }

        [Required]
        public string PasswordHash { get; set; }

        public virtual ICollection<SellerDBModel> Sellers { get; set; }
        public virtual ICollection<FeedbackDBModel> Feedbacks { get; set; }
    }

}
