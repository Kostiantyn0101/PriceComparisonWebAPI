using DLL.Context;
using Domain.Models.Response;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;


namespace DLL.Repository
{
    public class BaseRepository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        protected AppDbContext context;
        private DbSet<TEntity> entities;
        protected DbSet<TEntity> Entities => this.entities ?? context.Set<TEntity>();

        public BaseRepository(AppDbContext context) => this.context = context;

        public async Task<OperationDetailsResponseModel> CreateAsync(TEntity entity)
        {
            try
            {
                await Entities.AddAsync(entity);
                await context.SaveChangesAsync();
                return new OperationDetailsResponseModel { IsError = false, Message = "Entity Created" };
            }
            catch (Exception ex)
            {
                return new OperationDetailsResponseModel { IsError = true, Message = "Error", Exception = ex };
            }
        }

        public virtual async Task<IEnumerable<TEntity>> GetFromConditionAsync(Expression<Func<TEntity, bool>> condition) =>
            await Entities.Where(condition).ToListAsync().ConfigureAwait(false);

        public IQueryable<TEntity> GetQuery() => Entities.AsQueryable();

        public async Task<IEnumerable<TEntity>> ProcessQueryAsync(IQueryable<TEntity> query) => await query.ToListAsync();
    }
}
