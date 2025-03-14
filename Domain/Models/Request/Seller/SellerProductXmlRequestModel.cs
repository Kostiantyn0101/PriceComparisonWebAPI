using Microsoft.AspNetCore.Http;

namespace Domain.Models.Request.Seller
{
    public class SellerProductXmlRequestModel
    {
        public IFormFile? PriceXML { get; set; }
    }
}
