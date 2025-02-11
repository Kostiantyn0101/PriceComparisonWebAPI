using System.Linq.Expressions;
using Domain.Models.DBModels;
using Domain.Models.Request.Products;
using Domain.Models.Response;
using Domain.Models.Response.Products;

namespace BLL.Services.ProductService
{
    public interface IProductVideoService
    {
        Task<OperationResultModel<bool>> CreateAsync(ProductVideoCreateRequestModel model);
        Task<OperationResultModel<bool>> UpdateAsync(ProductVideoUpdateRequestModel entity);
        Task<OperationResultModel<bool>> DeleteAsync(int id);
        IQueryable<ProductVideoDBModel> GetQuery();
        Task<IEnumerable<ProductVideoResponseModel>> GetFromConditionAsync(Expression<Func<ProductVideoDBModel, bool>> condition);
        Task<IEnumerable<ProductVideoDBModel>> ProcessQueryAsync(IQueryable<ProductVideoDBModel> query);
    }
}
