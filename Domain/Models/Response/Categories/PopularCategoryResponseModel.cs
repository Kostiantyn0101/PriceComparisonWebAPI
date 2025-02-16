
using Domain.Models.DBModels;
using Domain.Models.Response.Products;

namespace Domain.Models.Response.Categories
{
    public class PopularCategoryResponseModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public ICollection<PopularProductResponseModel>? Products { get; set; }
    }
}
