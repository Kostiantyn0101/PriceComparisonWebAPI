namespace Domain.Models.DBModels
{
    public class CategoryCharacteristicDBModel
    {
        public int CategoryId { get; set; }
        public CategoryDBModel Category { get; set; }

        public int CharacteristicId { get; set; }
        public CharacteristicDBModel Characteristic { get; set; }
    }
}
