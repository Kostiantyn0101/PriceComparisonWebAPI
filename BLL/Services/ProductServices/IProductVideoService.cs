using Domain.Models.DBModels;
using Domain.Models.Request.Products;
using Domain.Models.Response;
using Domain.Models.Response.Products;
using System.Linq.Expressions;

namespace BLL.Services.ProductServices
{
    public interface IProductVideoService
    {
        Task<OperationResultModel<ProductVideoDBModel>> CreateAsync(ProductVideoCreateRequestModel model);
        Task<OperationResultModel<ProductVideoDBModel>> UpdateAsync(ProductVideoUpdateRequestModel entity);
        Task<OperationResultModel<bool>> DeleteAsync(int id);
        IQueryable<ProductVideoDBModel> GetQuery();
        Task<IEnumerable<ProductVideoResponseModel>> GetFromConditionAsync(Expression<Func<ProductVideoDBModel, bool>> condition);
        Task<IEnumerable<ProductVideoDBModel>> ProcessQueryAsync(IQueryable<ProductVideoDBModel> query);
    }
}
