using Domain.Models.Primitives;
using Domain.Models.Response;

namespace DLL.Repository
{
    public interface ICompositeKeyRepository<TEntity, TKey1, TKey2> : IRepository<TEntity, CompositeKey<TKey1, TKey2>>
         where TEntity : class, IEntity<CompositeKey<TKey1, TKey2>>
    {
        new Task<OperationResultModel<bool>> DeleteAsync(CompositeKey<TKey1, TKey2> id);
    }
}
