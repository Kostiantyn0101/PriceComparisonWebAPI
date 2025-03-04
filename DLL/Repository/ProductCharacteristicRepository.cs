using DLL.Context;
using Domain.Models.DBModels;
using Domain.Models.Exceptions;
using Domain.Models.Response;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace DLL.Repository.Abstractions
{
    //deprecated
    public class ProductCharacteristicRepository : IProductCharacteristicRepository
    {
        private readonly AppDbContext _context;

        public ProductCharacteristicRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<OperationDetailsResponseModel> CreateAsync(ProductCharacteristicDBModel entity)
        {
            try
            {
                _context.ProductCharacteristics.Add(entity);
                await _context.SaveChangesAsync();
                return new OperationDetailsResponseModel() { IsError = false, Message = "Create success", Exception = null };
            }
            catch (Exception ex)
            {
                return new OperationDetailsResponseModel() { IsError = true, Message = "Create error", Exception = ex };
            }
        }
        public async Task<OperationDetailsResponseModel> DeleteAsync(int productId, int characteristicId)
        {
            try
            {
                var entity = await _context.ProductCharacteristics
                                      .FirstOrDefaultAsync(pc => pc.ProductId == productId &&
                                                                 pc.CharacteristicId == characteristicId);
                if (entity == null)
                { 
                    return new OperationDetailsResponseModel
                    {
                        IsError = true,
                        Message = "Entity not found",
                        Exception = new EntityNotFoundException("Product not found")
                    };
                }

                _context.ProductCharacteristics.Remove(entity);
                await _context.SaveChangesAsync();
                return new OperationDetailsResponseModel() { IsError = false, Message = "Delete success", Exception = null };
            }
            catch (Exception ex)
            {
                return new OperationDetailsResponseModel() { IsError = true, Message = "Delete error", Exception = ex };
            }
        }

        public async Task<OperationDetailsResponseModel> UpdateAsync(ProductCharacteristicDBModel entity)
        {
            try
            {
                _context.ProductCharacteristics.Update(entity);
                await _context.SaveChangesAsync();
                return new OperationDetailsResponseModel() { IsError = false, Message = "Update success", Exception = null };
            }
            catch (Exception ex)
            {
                return new OperationDetailsResponseModel() { IsError = true, Message = "Update error", Exception = ex };
            }
        }

        public virtual async Task<IEnumerable<ProductCharacteristicDBModel>> GetFromConditionAsync(Expression<Func<ProductCharacteristicDBModel, bool>> condition) =>
            await _context.ProductCharacteristics.Where(condition).ToListAsync().ConfigureAwait(false);

        public IQueryable<ProductCharacteristicDBModel> GetQuery() => _context.ProductCharacteristics.AsQueryable();

        public async Task<IEnumerable<ProductCharacteristicDBModel>> ProcessQueryAsync(IQueryable<ProductCharacteristicDBModel> query) => 
            await query.ToListAsync();
    }
}
