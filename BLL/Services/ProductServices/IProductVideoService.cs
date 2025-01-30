using System.Linq.Expressions;
using Domain.Models.DBModels;
using Domain.Models.Response;

namespace BLL.Services.ProductService
{
    public interface IProductVideoService
    {
        Task<OperationDetailsResponseModel> CreateAsync(ProductVideoDBModel model);
        Task<OperationDetailsResponseModel> UpdateAsync(ProductVideoDBModel entity);
        Task<OperationDetailsResponseModel> DeleteAsync(int id);
        IQueryable<ProductVideoDBModel> GetQuery();
        Task<IEnumerable<ProductVideoDBModel>> GetFromConditionAsync(Expression<Func<ProductVideoDBModel, bool>> condition);
        Task<IEnumerable<ProductVideoDBModel>> ProcessQueryAsync(IQueryable<ProductVideoDBModel> query);
    }
}
