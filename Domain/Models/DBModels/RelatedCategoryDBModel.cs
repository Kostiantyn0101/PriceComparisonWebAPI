using Domain.Models.Primitives;

namespace Domain.Models.DBModels
{
    public class RelatedCategoryDBModel : IEntity<CompositeKey<int, int>>
    {
        public CompositeKey<int, int> Id
        {
            get => new CompositeKey<int, int> { Key1 = this.CategoryId, Key2 = this.RelatedCategoryId };
            set
            {
                if (value != null)
                {
                    CategoryId = value.Key1;
                    RelatedCategoryId = value.Key2;
                }
            }
        }
        public int CategoryId { get; set; }
        public CategoryDBModel Category { get; set; }

        public int RelatedCategoryId { get; set; }
        public CategoryDBModel RelatedCategoryItem { get; set; }
    }
}
