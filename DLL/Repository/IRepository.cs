using Domain.Models.Primitives;
using Domain.Models.Response;
using System.Linq.Expressions;

namespace DLL.Repository
{
    public interface IRepository<TEntity, TKey> where TEntity : IEntity<TKey>
    {
        Task<OperationResultModel<TEntity>> CreateAsync(TEntity entity);
        Task<OperationResultModel<TEntity>> UpdateAsync(TEntity entityNew);
        Task<OperationResultModel<bool>> DeleteAsync(TKey id);

        Task<IEnumerable<TEntity>> GetFromConditionAsync(Expression<Func<TEntity, bool>> condition);
        Task<IEnumerable<TEntity>> ProcessQueryAsync(IQueryable<TEntity> query);
        IQueryable<TEntity> GetQuery();
    }
}
