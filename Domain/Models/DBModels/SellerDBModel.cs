namespace Domain.Models.DBModels
{
    public class SellerDBModel
    {
        public int Id { get; set; }
        public string ApiKey { get; set; }
        public string? LogoImageUrl { get; set; }
        public string? WebsiteUrl { get; set; }
        public bool IsActive { get; set; }

        public int UserId { get; set; }
        public UserDBModel User { get; set; }

        public ICollection<PriceDBModel> Prices { get; set; }
        public ICollection<PriceHistoryDBModel> PriceHistories { get; set; }

    }

}
