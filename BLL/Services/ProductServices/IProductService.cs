using System.Linq.Expressions;
using Domain.Models.DBModels;
using Domain.Models.Request.Products;
using Domain.Models.Response;
using Domain.Models.Response.Products;

namespace BLL.Services.ProductService
{
    public interface IProductService
    {
        Task<OperationDetailsResponseModel> CreateAsync(ProductRequestModel model);
        Task<OperationDetailsResponseModel> UpdateAsync(ProductRequestModel entity);
        Task<OperationDetailsResponseModel> DeleteAsync(int id);
        IQueryable<ProductDBModel> GetQuery();
        Task<IEnumerable<ProductResponseModel>> GetFromConditionAsync(Expression<Func<ProductDBModel, bool>> condition);
        Task<IEnumerable<ProductDBModel>> ProcessQueryAsync(IQueryable<ProductDBModel> query);
    }
}
