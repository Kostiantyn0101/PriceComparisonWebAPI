using System.Linq.Expressions;
using Domain.Models.DBModels;
using Domain.Models.Response;

namespace BLL.Services.ProductServices
{
    public interface ICharacteristicService
    {
        Task<OperationDetailsResponseModel> CreateAsync(CharacteristicDBModel model);
        Task<OperationDetailsResponseModel> UpdateAsync(CharacteristicDBModel entity);
        Task<OperationDetailsResponseModel> DeleteAsync(int id);
        IQueryable<CharacteristicDBModel> GetQuery();
        Task<IEnumerable<CharacteristicDBModel>> GetFromConditionAsync(Expression<Func<CharacteristicDBModel, bool>> condition);
        Task<IEnumerable<CharacteristicDBModel>> ProcessQueryAsync(IQueryable<CharacteristicDBModel> query);
    }
}
