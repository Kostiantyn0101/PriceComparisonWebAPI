
namespace Domain.Models.Request.Categories
{
    public class CategoryCharacteristicGroupCreateRequestModel
    {
        public int CategoryId { get; set; }
        public int CharacteristicGroupId { get; set; }
        public int GroupDisplayOrder { get; set; }
    }
}
