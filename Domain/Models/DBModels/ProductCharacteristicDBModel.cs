using Domain.Models.Primitives;

namespace Domain.Models.DBModels
{
    public class ProductCharacteristicDBModel : IEntity<CompositeKey<int, int>>
    {
        public CompositeKey<int, int> Id
        {
            get => new CompositeKey<int, int> { Key1 = this.ProductId, Key2 = this.CharacteristicId };
            set
            {
                if (value != null)
                {
                    ProductId = value.Key1;
                    CharacteristicId = value.Key2;
                }
            }
        }
        public int ProductId { get; set; }
        public ProductDBModel Product { get; set; }

        public int CharacteristicId { get; set; }
        public CharacteristicDBModel Characteristic { get; set; }

        public string? ValueText { get; set; }
        public decimal? ValueNumber { get; set; }
        public bool? ValueBoolean { get; set; }
        public DateTime? ValueDate { get; set; }
    }

}
