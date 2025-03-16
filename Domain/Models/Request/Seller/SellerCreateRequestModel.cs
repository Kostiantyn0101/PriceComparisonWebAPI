using Microsoft.AspNetCore.Http;

namespace Domain.Models.Request.Seller
{
    public class SellerCreateRequestModel
    {
        public string StoreName { get; set; }
        public IFormFile? LogoImage { get; set; }
        public string WebsiteUrl { get; set; }
        public bool IsActive { get; set; }
        public int UserId { get; set; }
        public decimal AccountBalance { get; set; }
        public bool PublishPriceList { get; set; }
    }
}
