using Domain.Models.Primitives;

namespace Domain.Models.DBModels
{
    public class FilterDBModel : IEntity<int>
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string ShortTitle { get; set; }
        public string Description { get; set; }
        public ICollection<FilterCriterionDBModel> Criteria { get; set; }
        public ICollection<ProductFilterDBModel> ProductFilters { get; set; }
    }
}
