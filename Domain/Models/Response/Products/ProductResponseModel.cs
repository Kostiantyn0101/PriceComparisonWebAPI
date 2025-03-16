using System.Drawing;

namespace Domain.Models.Response.Products
{
    public class ProductResponseModel
    {
        public string Title { get; set; } //to delete
        public int ProductId { get; set; }
        public string? ModelNumber { get; set; }
        public int? ProductGroupId { get; set; }
        public bool IsDefault { get; set; }
        public List<ColorResponseModel>? Colors { get; set; }
    }
}
