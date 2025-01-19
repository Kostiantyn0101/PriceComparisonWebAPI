using Domain.Models.Response;
using System.Linq.Expressions;

namespace DLL.Repository
{
    public interface IRepository<TEntity>
    {
        Task<OperationDetailsResponseModel> CreateAsync(TEntity entity);
        Task<IEnumerable<TEntity>> GetFromConditionAsync(Expression<Func<TEntity, bool>> condition);
        IQueryable<TEntity> GetQuery();
        Task<IEnumerable<TEntity>> ProcessQueryAsync(IQueryable<TEntity> query);
    }
}
