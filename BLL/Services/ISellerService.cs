using System.Linq.Expressions;
using Domain.Models.DBModels;
using Domain.Models.Response;

namespace BLL.Services
{
    public interface ISellerService
    {
        Task<OperationDetailsResponseModel> CreateAsync(SellerDBModel model);
        Task<OperationDetailsResponseModel> UpdateAsync(SellerDBModel entity);
        Task<OperationDetailsResponseModel> DeleteAsync(int id);
        IQueryable<SellerDBModel> GetQuery();
        Task<IEnumerable<SellerDBModel>> GetFromConditionAsync(Expression<Func<SellerDBModel, bool>> condition);
        Task<IEnumerable<SellerDBModel>> ProcessQueryAsync(IQueryable<SellerDBModel> query);
    }
}
