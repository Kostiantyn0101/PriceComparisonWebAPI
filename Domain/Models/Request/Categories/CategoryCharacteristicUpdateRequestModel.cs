namespace Domain.Models.Request.Categories
{
    public class CategoryCharacteristicUpdateRequestModel
    {
        public int OldCategoryId { get; set; }
        public int OldCharacteristicId { get; set; }

        public int NewCategoryId { get; set; }
        public int NewCharacteristicId { get; set; }
    }
}
