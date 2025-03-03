using Domain.Models.Primitives;

namespace Domain.Models.DBModels
{
    public class CategoryCharacteristicDBModel : IEntity<CompositeKey<int, int>>
    {
        public CompositeKey<int, int> Id
        {
            get => new CompositeKey<int, int> { Key1 = this.CategoryId, Key2 = this.CharacteristicId };
            set
            {
                if (value != null)
                {
                    CategoryId = value.Key1;
                    CharacteristicId = value.Key2;
                }
            }
        }
        public int CategoryId { get; set; }
        public CategoryDBModel Category { get; set; }

        public int CharacteristicId { get; set; }
        public CharacteristicDBModel Characteristic { get; set; }
    }
}
