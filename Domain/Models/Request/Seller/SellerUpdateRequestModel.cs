namespace Domain.Models.Request.Seller
{
    public class SellerUpdateRequestModel
    {
        public int Id { get; set; }
        public string ApiKey { get; set; }
        public string StoreName { get; set; }
        public string? LogoImageUrl { get; set; }
        public string WebsiteUrl { get; set; }
        public bool IsActive { get; set; }
        public int UserId { get; set; }
        public decimal AccoundBalance { get; set; }
    }
}
