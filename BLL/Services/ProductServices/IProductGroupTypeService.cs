using System.Linq.Expressions;
using Domain.Models.DBModels;
using Domain.Models.Request.Products;
using Domain.Models.Response;
using Domain.Models.Response.Products;

namespace BLL.Services.ProductServices
{
    public interface IProductGroupTypeService
    {
        Task<OperationResultModel<ProductGroupTypeDBModel>> CreateAsync(ProductGroupTypeCreateRequestModel request);
        Task<OperationResultModel<ProductGroupTypeDBModel>> UpdateAsync(ProductGroupTypeUpdateRequestModel request);
        Task<OperationResultModel<bool>> DeleteAsync(int id);
        IQueryable<ProductGroupTypeDBModel> GetQuery();
        Task<IEnumerable<ProductGroupTypeResponseModel>> GetFromConditionAsync(Expression<Func<ProductGroupTypeDBModel, bool>> condition);
        Task<IEnumerable<ProductGroupTypeDBModel>> ProcessQueryAsync(IQueryable<ProductGroupTypeDBModel> query);
    }
}
