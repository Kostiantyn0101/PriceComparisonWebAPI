using System.Linq.Expressions;
using Domain.Models.DBModels;
using Domain.Models.Request.Products;
using Domain.Models.Response;
using Domain.Models.Response.Categories;
using Domain.Models.Response.Products;

namespace BLL.Services.ProductServices
{
    public interface IPopularProductService
    {
        Task RegisterClickAsync(int productId);
        Task<IEnumerable<PopularCategoryResponseModel>> GetPopularCategories();
        Task<IEnumerable<PopularProductResponseModel>> GetPopularProductsByCategory(int categoryId);
        Task<IEnumerable<PopularCategoryResponseModel>> GetPopularCategoriesWithProducts();

        //Task<OperationResultModel<bool>> UpdateAsync(ReviewUpdateRequestModel request);
        //Task<OperationResultModel<bool>> DeleteAsync(int id);
        //IQueryable<ReviewDBModel> GetQuery();
        //Task<IEnumerable<ReviewResponseModel>> GetFromConditionAsync(Expression<Func<ReviewDBModel, bool>> condition);
        //Task<IEnumerable<ReviewDBModel>> ProcessQueryAsync(IQueryable<ReviewDBModel> query);
    }
}
