using Domain.Models.Primitives;

namespace Domain.Models.DBModels
{
    public class ProductImageDBModel : EntityDBModel
    {
        public int ProductId { get; set; }
        public ProductDBModel Product { get; set; }

        public string ImageUrl { get; set; }
        public bool IsPrimary { get; set; }
    }
}
