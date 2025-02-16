namespace Domain.Models.Request.Categories
{
    public class RelatedCategoryUpdateRequestModel
    {
        public int OldCategoryId { get; set; }
        public int OldRelatedCategoryId { get; set; }

        public int NewCategoryId { get; set; }
        public int NewRelatedCategoryId { get; set; }
    }
}
