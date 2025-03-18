using DLL.Context;
using Domain.Models.Primitives;
using Domain.Models.Response;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace DLL.Repository.Abstractions
{
    public class Repository<TEntity, TKey> : IRepository<TEntity, TKey> where TEntity : class, IEntity<TKey>
    {
        protected readonly AppDbContext _context;
        protected readonly DbSet<TEntity> Entities;

        public Repository(AppDbContext context)
        {
            _context = context;
            Entities = _context.Set<TEntity>();
        }

        public virtual async Task<OperationResultModel<TEntity>> CreateAsync(TEntity entity)
        {
            try
            {
                Entities.Add(entity);
                await _context.SaveChangesAsync();
                return OperationResultModel<TEntity>.Success(entity);
            }
            catch (Exception ex)
            {
                return OperationResultModel<TEntity>.Failure("Create error", ex);
            }
        }
        public virtual async Task<OperationResultModel<bool>> DeleteAsync(TKey id)
        {
            try
            {
                var toDelete = await Entities
                    //.FirstOrDefaultAsync(x => EqualityComparer<TKey>.Default.Equals(x.Id, id));
                    .FirstOrDefaultAsync(x => x.Id!.Equals(id));

                if (toDelete == null)
                {
                    return OperationResultModel<bool>.Failure("Entity not found", new Exception("Entity not found"));
                }

                Entities.Remove(toDelete);
                await _context.SaveChangesAsync();
                return OperationResultModel<bool>.Success(true);
            }
            catch (Exception ex)
            {
                return OperationResultModel<bool>.Failure("Delete error", ex);
            }
        }

        public virtual async Task<OperationResultModel<TEntity>> UpdateAsync(TEntity entity)
        {
            try
            {
                _context.Entry(entity).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return OperationResultModel<TEntity>.Success(entity);
            }
            catch (Exception ex)
            {
                return OperationResultModel<TEntity>.Failure("Update error", ex);
            }
        }

        public virtual async Task<IEnumerable<TEntity>> GetFromConditionAsync(Expression<Func<TEntity, bool>> condition) =>
            await Entities.Where(condition).ToListAsync().ConfigureAwait(false);

        public virtual IQueryable<TEntity> GetQuery() => Entities.AsQueryable();

        public virtual async Task<IEnumerable<TEntity>> ProcessQueryAsync(IQueryable<TEntity> query) => await query.ToListAsync();
    }
}
