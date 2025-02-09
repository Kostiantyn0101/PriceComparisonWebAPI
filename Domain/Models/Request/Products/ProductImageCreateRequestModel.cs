using Microsoft.AspNetCore.Http;

namespace Domain.Models.Request.Products
{
    public class ProductImageCreateRequestModel
    {
        public int ProductId { get; set; }
        public List<IFormFile> Images { get; set; }
    }
}

