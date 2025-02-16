using Domain.Models.Primitives;

namespace Domain.Models.DBModels
{
    public class CharacteristicDBModel : EntityDBModel
    {
        public string Title { get; set; }
        public string DataType { get; set; }
        public string? Unit { get; set; }

        public int CharacteristicGroupId { get; set; }
        public CharacteristicGroupDBModel CharacteristicGroup { get; set; } //+ +

        public int DisplayOrder { get; set; } // less is gigger
        public bool IncludeInShortDescription { get; set; } 

        public ICollection<CategoryCharacteristicDBModel> CategoryCharacteristics { get; set; }
        public ICollection<ProductCharacteristicDBModel> ProductCharacteristics { get; set; }
       
    }
}
