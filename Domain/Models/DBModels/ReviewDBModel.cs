using Domain.Models.Primitives;

namespace Domain.Models.DBModels
{
    public class ReviewDBModel : EntityDBModel
    {
        public string ReviewUrl { get; set; }

        public int ProductId { get; set; }
        public ProductDBModel Product { get; set; }

    }
}
