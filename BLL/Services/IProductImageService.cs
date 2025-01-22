using System.Linq.Expressions;
using Domain.Models.DBModels;
using Domain.Models.Response;

namespace BLL.Services
{
    public interface IProductImageService
    {
        Task<OperationDetailsResponseModel> CreateAsync(ProductImageDBModel model);
        Task<OperationDetailsResponseModel> UpdateAsync(ProductImageDBModel entity);
        Task<OperationDetailsResponseModel> DeleteAsync(int id);
        IQueryable<ProductImageDBModel> GetQuery();
        Task<IEnumerable<ProductImageDBModel>> GetFromConditionAsync(Expression<Func<ProductImageDBModel, bool>> condition);
        Task<IEnumerable<ProductImageDBModel>> ProcessQueryAsync(IQueryable<ProductImageDBModel> query);
    }
}
