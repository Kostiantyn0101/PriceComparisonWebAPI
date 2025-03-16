using Microsoft.AspNetCore.Http;

namespace Domain.Models.Request.Seller
{
    public class SellerProductDetailsRequestModel
    {
        public int BaseProductId { get; set; }
        public int ProductGroupId { get; set; }
    }
}
