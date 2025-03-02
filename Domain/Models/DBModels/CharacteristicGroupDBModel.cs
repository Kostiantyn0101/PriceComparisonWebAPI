using Domain.Models.Primitives;

namespace Domain.Models.DBModels
{
    public class CharacteristicGroupDBModel : IEntity<int>
    {
        public int Id { get; set; }
        public string Title { get; set; }

        public ICollection<CharacteristicDBModel> Characteristics { get; set; }
        public ICollection<CategoryCharacteristicGroupDBModel> CategoryCharacteristicGroups { get; set; }
    }
}
