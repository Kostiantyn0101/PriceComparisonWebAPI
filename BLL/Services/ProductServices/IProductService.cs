using System.Linq.Expressions;
using Domain.Models.DBModels;
using Domain.Models.Request.Products;
using Domain.Models.Response;
using Domain.Models.Response.Products;

namespace BLL.Services.ProductServices
{
    public interface IProductService
    {
        Task<OperationResultModel<ProductDBModel>> CreateAsync(ProductCreateRequestModel model);
        Task<OperationResultModel<ProductDBModel>> UpdateAsync(ProductUpdateRequestModel entity);
        Task<OperationResultModel<bool>> DeleteAsync(int id);
        Task<OperationResultModel<PaginatedResponse<ProductResponseModel>>> GetPaginatedProductsAsync(
            Expression<Func<ProductDBModel, bool>> condition, int page, int pageSize);
        Task<OperationResultModel<PaginatedResponse<BaseProductByCategoryResponseModel>>> GetPaginatedProductsByCategoryAsync(
              int categoryId, int page, int pageSize);
        IQueryable<ProductDBModel> GetQuery();
        Task<IEnumerable<ProductResponseModel>> GetFromConditionAsync(Expression<Func<ProductDBModel, bool>> condition);
        Task<IEnumerable<ProductDBModel>> ProcessQueryAsync(IQueryable<ProductDBModel> query);
        Task<IEnumerable<ProductSearchResponseModel>> SearchByFullNameOrModelAsync(string name);
    }
}
