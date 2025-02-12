using System.Linq.Expressions;
using Domain.Models.DBModels;
using Domain.Models.Request.Products;
using Domain.Models.Response;
using Domain.Models.Response.Products;

namespace BLL.Services.ProductService
{
    public interface IProductService
    {
        Task<OperationResultModel<bool>> CreateAsync(ProductRequestModel model);
        Task<OperationResultModel<bool>> UpdateAsync(ProductRequestModel entity);
        Task<OperationResultModel<bool>> DeleteAsync(int id);
        IQueryable<ProductDBModel> GetQuery();
        Task<IEnumerable<ProductResponseModel>> GetFromConditionAsync(Expression<Func<ProductDBModel, bool>> condition);
        Task<IEnumerable<ProductDBModel>> ProcessQueryAsync(IQueryable<ProductDBModel> query);
    }
}
