using Domain.Models.Primitives;

namespace Domain.Models.DBModels
{
    public class CharacteristicDBModel : EntityDBModel
    {
        public string Title { get; set; }
        public string DataType { get; set; }
        public string? Unit { get; set; }

        public ICollection<CategoryCharacteristicDBModel> CategoryCharacteristics { get; set; }
        public ICollection<ProductCharacteristicDBModel> ProductCharacteristics { get; set; }
    }
}
