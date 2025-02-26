namespace Domain.Models.Response.Gpt.Product
{
    public class SimplifiedProductCharacteristicGroupResponseModel
    {
        public string CharacteristicGroupTitle { get; set; }
        public List<SimplifiedProductCharacteristicResponseModel> ProductCharacteristics { get; set; }
    }
}
