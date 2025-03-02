using Domain.Models.Primitives;

namespace Domain.Models.DBModels
{
    public class CategoryCharacteristicGroupDBModel : IEntity<int>
    {
        public int Id { get; set; }
        public int CategoryId { get; set; }
        public CategoryDBModel Category { get; set; }

        public int CharacteristicGroupId { get; set; }
        public CharacteristicGroupDBModel CharacteristicGroup { get; set; }

        public int GroupDisplayOrder { get; set; }
    }
}
