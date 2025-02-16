using System.ComponentModel.DataAnnotations;

namespace PriceComparisonWebAPI.Controllers.Category
{
    public class CategoryCharacteristicUpdateRequestModel
    {
        public int OldCategoryId { get; set; }
        public int OldCharacteristicId { get; set; }
        public int NewCategoryId { get; set; }
        public int NewCharacteristicId { get; set; }
    }
}
