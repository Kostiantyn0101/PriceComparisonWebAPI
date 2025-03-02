using Domain.Models.Primitives;

namespace Domain.Models.DBModels
{
    public class ProductClicksDBModel : IEntity<int>
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public DateTime ClickDate { get; set; }

        public ProductDBModel Product { get; set; }
    }
}
