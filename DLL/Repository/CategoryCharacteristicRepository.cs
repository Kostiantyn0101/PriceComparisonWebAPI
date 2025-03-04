using DLL.Context;
using Domain.Models.DBModels;
using Domain.Models.Exceptions;
using Domain.Models.Response;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace DLL.Repository.Abstractions
{
    //deprecated
    public class CategoryCharacteristicRepository : ICategoryCharacteristicRepository
    {
        private readonly AppDbContext _context;

        public CategoryCharacteristicRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<OperationDetailsResponseModel> CreateAsync(CategoryCharacteristicDBModel entity)
        {
            try
            {
                _context.CategoryCharacteristics.Add(entity);
                await _context.SaveChangesAsync();
                return new OperationDetailsResponseModel() { IsError = false, Message = "Create success", Exception = null };
            }
            catch (Exception ex)
            {
                return new OperationDetailsResponseModel() { IsError = true, Message = "Create error", Exception = ex };
            }
        }
        public async Task<OperationDetailsResponseModel> DeleteAsync(int categoryId, int characteristicId)
        {
            try
            {
                var entity = await _context.CategoryCharacteristics
                                      .FirstOrDefaultAsync(cc => cc.CategoryId == categoryId &&
                                                                 cc.CharacteristicId == characteristicId);
                if (entity == null)
                {
                    return new OperationDetailsResponseModel
                    {
                        IsError = true,
                        Message = "Entity not found",
                        Exception = new EntityNotFoundException("Characteristic not found")
                    };
                }

                _context.CategoryCharacteristics.Remove(entity);
                await _context.SaveChangesAsync();
                return new OperationDetailsResponseModel() { IsError = false, Message = "Delete success", Exception = null };
            }
            catch (Exception ex)
            {
                return new OperationDetailsResponseModel() { IsError = true, Message = "Delete error", Exception = ex };
            }
        }

        public async Task<OperationDetailsResponseModel> UpdateAsync(CategoryCharacteristicDBModel entity)
        {
            try
            {
                _context.CategoryCharacteristics.Update(entity);
                await _context.SaveChangesAsync();
                return new OperationDetailsResponseModel() { IsError = false, Message = "Update success", Exception = null };
            }
            catch (Exception ex)
            {
                return new OperationDetailsResponseModel() { IsError = true, Message = "Update error", Exception = ex };
            }
        }

        public virtual async Task<IEnumerable<CategoryCharacteristicDBModel>> GetFromConditionAsync(Expression<Func<CategoryCharacteristicDBModel, bool>> condition) =>
            await _context.CategoryCharacteristics.Where(condition).ToListAsync().ConfigureAwait(false);

        public IQueryable<CategoryCharacteristicDBModel> GetQuery() => _context.CategoryCharacteristics.AsQueryable();

        public async Task<IEnumerable<CategoryCharacteristicDBModel>> ProcessQueryAsync(IQueryable<CategoryCharacteristicDBModel> query) => 
            await query.ToListAsync();
    }
}
