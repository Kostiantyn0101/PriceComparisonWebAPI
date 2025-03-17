using Domain.Models.Primitives;

namespace Domain.Models.DBModels
{
    public class ProductGroupDBModel : IEntity<int>
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int? ProductGroupTypeId { get; set; } // delete ?
        public int DisplayOrder { get; set; }

        public ProductGroupTypeDBModel? ProductGroupType { get; set; } // delete ?
        public IEnumerable<ProductDBModel> Products { get; set; }
    }
}
