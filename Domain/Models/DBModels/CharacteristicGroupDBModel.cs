using Domain.Models.Primitives;

namespace Domain.Models.DBModels
{
    public class CharacteristicGroupDBModel : EntityDBModel
    {
        public string Title { get; set; }

        public ICollection<CharacteristicDBModel> Characteristics { get; set; } //+ +
        public ICollection<CategoryCharacteristicGroupDBModel> CategoryCharacteristicGroups { get; set; } //+ +
    }
}
