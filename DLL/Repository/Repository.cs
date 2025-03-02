using DLL.Context;
using Domain.Models.Primitives;
using Domain.Models.Response;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace DLL.Repository.Abstractions
{
    public class Repository<TEntity>(AppDbContext context) : IRepository<TEntity> where TEntity : EntityDBModel
    {
        private readonly AppDbContext _context = context;
        protected DbSet<TEntity> Entities => _context.Set<TEntity>();

        public async Task<OperationResultModel<TEntity>> CreateAsync(TEntity entity)
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
        public async Task<OperationDetailsResponseModel> DeleteAsync(int id)
        {
            try
            {
                var toDelete = await Entities.Where(x => x.Id == id).FirstOrDefaultAsync();
                Entities.Remove(toDelete);
                await _context.SaveChangesAsync();
                return new OperationDetailsResponseModel() { IsError = false, Message = "Delete success", Exception = null };
            }
            catch (Exception ex)
            {
                return new OperationDetailsResponseModel() { IsError = true, Message = "Delete error", Exception = ex };
            }
        }

        public async Task<OperationDetailsResponseModel> UpdateAsync(TEntity entity)
        {
            try
            {
                _context.Entry(entity).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return new OperationDetailsResponseModel() { IsError = false, Message = "Update success", Exception = null };
            }
            catch (Exception ex)
            {
                return new OperationDetailsResponseModel() { IsError = true, Message = "Update error", Exception = ex };
            }
        }

        public virtual async Task<IEnumerable<TEntity>> GetFromConditionAsync(Expression<Func<TEntity, bool>> condition) =>
            await Entities.Where(condition).ToListAsync().ConfigureAwait(false);

        public IQueryable<TEntity> GetQuery() => Entities.AsQueryable();

        public async Task<IEnumerable<TEntity>> ProcessQueryAsync(IQueryable<TEntity> query) => await query.ToListAsync();
    }
}
