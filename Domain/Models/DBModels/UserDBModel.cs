namespace Domain.Models.DBModels
{
    public class UserDBModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }

        public int RoleId { get; set; }
        public RoleDBModel Role { get; set; }

        public ICollection<SellerDBModel> Sellers { get; set; }
        public ICollection<FeedbackDBModel> Feedbacks { get; set; }
    }

}
