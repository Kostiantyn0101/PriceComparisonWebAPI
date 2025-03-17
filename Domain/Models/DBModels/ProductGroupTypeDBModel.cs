using Domain.Models.Primitives;

namespace Domain.Models.DBModels
{
    public class ProductGroupTypeDBModel : IEntity<int>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        
        public IEnumerable<ProductGroupDBModel> ProductGroups { get; set; }
    }
}
