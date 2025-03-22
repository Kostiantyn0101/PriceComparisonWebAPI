using Domain.Models.Primitives;

namespace Domain.Models.DBModels
{
    public class ProductGroupDBModel : IEntity<int>
    {
        private string _name;

        public int Id { get; set; }
        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                NormalizedName = value.Trim().ToUpperInvariant();
            }
        }
        public string NormalizedName { get; private set; }
        public int ProductGroupTypeId { get; set; } 
        public int DisplayOrder { get; set; }

        public ProductGroupTypeDBModel ProductGroupType { get; set; } 
        public IEnumerable<ProductDBModel> Products { get; set; }
    }
}
