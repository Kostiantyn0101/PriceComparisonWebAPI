using System.Linq.Expressions;
using Domain.Models.DBModels;
using Domain.Models.Response;

namespace BLL.Services.ProductService
{
    public interface IProductSellerLinkService
    {
        Task<OperationDetailsResponseModel> CreateAsync(ProductSellerLinkDBModel model);
        Task<OperationDetailsResponseModel> UpdateAsync(ProductSellerLinkDBModel entity);
        Task<OperationDetailsResponseModel> DeleteAsync(int id);
        IQueryable<ProductSellerLinkDBModel> GetQuery();
        Task<IEnumerable<ProductSellerLinkDBModel>> GetFromConditionAsync(Expression<Func<ProductSellerLinkDBModel, bool>> condition);
        Task<IEnumerable<ProductSellerLinkDBModel>> ProcessQueryAsync(IQueryable<ProductSellerLinkDBModel> query);
    }
}
