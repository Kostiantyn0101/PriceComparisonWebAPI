using Domain.Models.Primitives;

namespace Domain.Models.DBModels
{
    public class ProductGroupDBModel : EntityDBModel
    {
        public int ProductId { get; set; }
        public ProductDBModel Product { get; set; }

        public string ProductGroupId { get; set; }
    }
}
