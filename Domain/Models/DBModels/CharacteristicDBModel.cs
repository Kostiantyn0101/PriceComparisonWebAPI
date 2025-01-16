namespace Domain.Models.DBModels
{
    public class CharacteristicDBModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string DataType { get; set; }
        public string? Unit { get; set; }

        public ICollection<CategoryCharacteristicDBModel> CategoryCharacteristics { get; set; }
        public ICollection<ProductCharacteristicDBModel> ProductCharacteristics { get; set; }
    }
}
