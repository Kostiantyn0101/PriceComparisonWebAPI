using Domain.Models.Primitives;

namespace Domain.Models.DBModels
{
    public class ProductVideoDBModel : EntityDBModel
    {
        public string VideoUrl { get; set; }

        public int ProductId { get; set; }
        public ProductDBModel Product { get; set; }
    }
}
