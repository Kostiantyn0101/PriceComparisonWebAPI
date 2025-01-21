using Domain.Models.Primitives;
using Domain.Models.Response;
using System.Linq.Expressions;

namespace DLL.Repository
{
    public interface IRepository<TEntity> where TEntity : EntityDBModel
    {
        Task<OperationDetailsResponseModel> CreateAsync(TEntity entity);
        Task<OperationDetailsResponseModel> UpdateAsync(TEntity entityNew);
        Task<OperationDetailsResponseModel> DeleteAsync(int id);

        Task<IEnumerable<TEntity>> GetFromConditionAsync(Expression<Func<TEntity, bool>> condition);
        Task<IEnumerable<TEntity>> ProcessQueryAsync(IQueryable<TEntity> query);
        IQueryable<TEntity> GetQuery();
    }
}
