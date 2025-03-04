using Domain.Models.DBModels;
using Domain.Models.Request.Products;
using Domain.Models.Response;
using Domain.Models.Response.Products;
using System.Linq.Expressions;

namespace BLL.Services.ProductServices
{
    public interface IProductImageService
    {
        Task<OperationResultModel<bool>> AddAsync(ProductImageCreateRequestModel model);
        Task<OperationResultModel<bool>> DeleteAsync(ProductImageDeleteRequestModel entity);
        Task<OperationResultModel<bool>> SetPrimaryImageAsync(ProductImageSetPrimaryRequestModel model);
        IQueryable<ProductImageDBModel> GetQuery();
        Task<IEnumerable<ProductImageResponseModel>> GetFromConditionAsync(Expression<Func<ProductImageDBModel, bool>> condition);
        Task<IEnumerable<ProductImageDBModel>> ProcessQueryAsync(IQueryable<ProductImageDBModel> query);
    }
}
