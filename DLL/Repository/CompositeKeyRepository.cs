using DLL.Context;
using DLL.Repository.Abstractions;
using Domain.Models.Primitives;
using Domain.Models.Response;
using Microsoft.EntityFrameworkCore;

namespace DLL.Repository
{
    public class CompositeKeyRepository<TEntity, TKey1, TKey2> : Repository<TEntity, CompositeKey<TKey1, TKey2>>,
                                                               ICompositeKeyRepository<TEntity, TKey1, TKey2> 
        where TEntity : class, IEntity<CompositeKey<TKey1, TKey2>>
    {
        public CompositeKeyRepository(AppDbContext context) : base(context)
        {
        }
        public override async Task<OperationDetailsResponseModel> DeleteAsync(CompositeKey<TKey1, TKey2> id)
        {
            try
            {
                var entity = await _context.Set<TEntity>()
                    .FirstOrDefaultAsync(x =>
                           EqualityComparer<TKey1>.Default.Equals(x.Id.Key1, id.Key1) &&
                           EqualityComparer<TKey2>.Default.Equals(x.Id.Key2, id.Key2));

                if (entity == null)
                {
                    return new OperationDetailsResponseModel
                    {
                        IsError = true,
                        Message = "Entity not found",
                        Exception = new Exception("Entity not found")
                    };
                }

                _context.Set<TEntity>().Remove(entity);
                await _context.SaveChangesAsync();
                return new OperationDetailsResponseModel { IsError = false, Message = "Delete success", Exception = null };
            }
            catch (Exception ex)
            {
                return new OperationDetailsResponseModel { IsError = true, Message = "Delete error", Exception = ex };
            }
        }
    }
}