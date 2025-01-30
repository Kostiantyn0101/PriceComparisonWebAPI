using System.Linq.Expressions;
using Domain.Models.DBModels;
using Domain.Models.Response;

namespace BLL.Services.ProductService
{
    public interface IProductService
    {
        Task<OperationDetailsResponseModel> CreateAsync(ProductDBModel model);
        Task<OperationDetailsResponseModel> UpdateAsync(ProductDBModel entity);
        Task<OperationDetailsResponseModel> DeleteAsync(int id);
        IQueryable<ProductDBModel> GetQuery();
        Task<IEnumerable<ProductDBModel>> GetFromConditionAsync(Expression<Func<ProductDBModel, bool>> condition);
        Task<IEnumerable<ProductDBModel>> ProcessQueryAsync(IQueryable<ProductDBModel> query);
    }
}
