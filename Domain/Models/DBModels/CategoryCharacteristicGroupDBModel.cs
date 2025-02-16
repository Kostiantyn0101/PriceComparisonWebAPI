using Domain.Models.Primitives;

namespace Domain.Models.DBModels
{
    public class CategoryCharacteristicGroupDBModel : EntityDBModel
    {
        public int CategoryId { get; set; }
        public CategoryDBModel Category { get; set; }

        public int CharacteristicGroupId { get; set; }
        public CharacteristicGroupDBModel CharacteristicGroup { get; set; }

        public int GroupDisplayOrder { get; set; }
    }
}
