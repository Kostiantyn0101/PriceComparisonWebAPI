//using Domain.Models.Primitives;

//namespace Domain.Models.DBModels
//{
//    public class CategoryFilterCriterionDBModel : IEntity<CompositeKey<int, int>>
//    {
//        public CompositeKey<int, int> Id
//        {
//            get => new CompositeKey<int, int> { Key1 = this.CategoryId, Key2 = this.FilterCriterionId };
//            set
//            {
//                if (value != null)
//                {
//                    CategoryId = value.Key1;
//                    FilterCriterionId = value.Key2;
//                }
//            }
//        }
//        public int CategoryId { get; set; }
//        public CategoryDBModel Category { get; set; }

//        public int FilterCriterionId { get; set; }
//        public FilterCriterionDBModel FilterCriterion { get; set; }
//    }
//}
