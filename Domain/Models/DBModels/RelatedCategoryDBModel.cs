namespace Domain.Models.DBModels
{
    public class RelatedCategoryDBModel
    {
        public int CategoryId { get; set; }
        public CategoryDBModel Category { get; set; }

        public int RelatedCategoryId { get; set; }
        public CategoryDBModel RelatedCategoryItem { get; set; }
    }
}
