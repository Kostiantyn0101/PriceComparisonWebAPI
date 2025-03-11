using Domain.Models.Primitives;

namespace Domain.Models.DBModels
{
    public class ReviewDBModel : IEntity<int>
    {
        public int Id { get; set; }
        public string ReviewUrl { get; set; }
        public int BaseProductId { get; set; }
        
        public BaseProductDBModel BaseProduct { get; set; }
    }
}
