using System.Drawing;

namespace Domain.Models.Response.Products
{
    public class ProductResponseModel
    {
        public int ProductId { get; set; }
        public string? ModelNumber { get; set; }
        public int? ProductGroupId { get; set; }
        public bool IsDefault { get; set; }
        public ColorResponseModel? Color { get; set; }
    }
}
