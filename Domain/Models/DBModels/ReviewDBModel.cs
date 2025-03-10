using Domain.Models.Primitives;

namespace Domain.Models.DBModels
{
    public class ReviewDBModel : IEntity<int>
    {
        public int Id { get; set; }
        public string ReviewUrl { get; set; }

        public int ProductId { get; set; }
        public int BaseProductId { get; set; }
        public ProductDBModel Product { get; set; } // delete
        public BaseProductDBModel BaseProduct { get; set; }

    }
}
