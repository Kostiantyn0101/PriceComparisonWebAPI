using Domain.Models.Primitives;

namespace Domain.Models.DBModels
{
    public class ProductGroupDBModel : IEntity<int>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        
        public IEnumerable<ProductDBModel> Products { get; set; }
    }
}
