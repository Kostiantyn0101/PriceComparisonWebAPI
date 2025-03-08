using Domain.Models.Primitives;

namespace Domain.Models.DBModels
{
    public class ProductFilterDBModel : IEntity<CompositeKey<int, int>>
    {
        public CompositeKey<int, int> Id
        {
            get => new CompositeKey<int, int> { Key1 = this.ProductId, Key2 = this.FilterId };
            set
            {
                if (value != null)
                {
                    ProductId = value.Key1;
                    FilterId = value.Key2;
                }
            }
        }
        public int ProductId { get; set; }
        public ProductDBModel Product { get; set; }

        public int FilterId { get; set; }
        public FilterDBModel Filter { get; set; }
    }
}
