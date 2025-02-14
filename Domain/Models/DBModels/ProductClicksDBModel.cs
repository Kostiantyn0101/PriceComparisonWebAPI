using Domain.Models.Primitives;

namespace Domain.Models.DBModels
{
    public class ProductClicksDBModel : EntityDBModel
    {
        public int ProductId { get; set; }
        public DateTime ClickDate { get; set; }

        public ProductDBModel Product { get; set; }
    }
}
