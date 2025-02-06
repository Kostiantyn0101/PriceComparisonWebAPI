using DLL.Context;
using Domain.Models.DBModels;
using Domain.Models.Exceptions;
using Domain.Models.Response;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace DLL.Repository.Abstractions
{
    public class RelatedCategoryRepository : IRelatedCategoryRepository
    {
        private readonly AppDbContext _context;

        public RelatedCategoryRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<OperationDetailsResponseModel> CreateAsync(RelatedCategoryDBModel entity)
        {
            try
            {
                _context.RelatedCategories.Add(entity);
                await _context.SaveChangesAsync();
                return new OperationDetailsResponseModel() { IsError = false, Message = "Create success", Exception = null };
            }
            catch (Exception ex)
            {
                return new OperationDetailsResponseModel() { IsError = true, Message = "Create error", Exception = ex };
            }
        }
        public async Task<OperationDetailsResponseModel> DeleteAsync(int categoryId, int relatedCategoryId)
        {
            try
            {
                var entity = await _context.RelatedCategories
                                      .FirstOrDefaultAsync(rc => rc.CategoryId == categoryId &&
                                                                 rc.RelatedCategoryId == relatedCategoryId);
                if (entity == null)
                { 
                    return new OperationDetailsResponseModel { 
                        IsError = true, 
                        Message = "Entity not found",
                        Exception = new EntityNotFoundException("Related category not found")
                    };
                }    

                _context.RelatedCategories.Remove(entity);
                await _context.SaveChangesAsync();
                return new OperationDetailsResponseModel() { IsError = false, Message = "Delete success", Exception = null };
            }
            catch (Exception ex)
            {
                return new OperationDetailsResponseModel() { IsError = true, Message = "Delete error", Exception = ex };
            }
        }

        public async Task<OperationDetailsResponseModel> UpdateAsync(RelatedCategoryDBModel entity)
        {
            try
            {
                _context.RelatedCategories.Update(entity);
                await _context.SaveChangesAsync();
                return new OperationDetailsResponseModel() { IsError = false, Message = "Update success", Exception = null };
            }
            catch (Exception ex)
            {
                return new OperationDetailsResponseModel() { IsError = true, Message = "Update error", Exception = ex };
            }
        }

        public virtual async Task<IEnumerable<RelatedCategoryDBModel>> GetFromConditionAsync(Expression<Func<RelatedCategoryDBModel, bool>> condition) =>
            await _context.RelatedCategories.Where(condition).ToListAsync().ConfigureAwait(false);

        public IQueryable<RelatedCategoryDBModel> GetQuery() => _context.RelatedCategories.AsQueryable();

        public async Task<IEnumerable<RelatedCategoryDBModel>> ProcessQueryAsync(IQueryable<RelatedCategoryDBModel> query) => 
            await query.ToListAsync();
    }
}
