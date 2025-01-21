using DLL.Context;
using Domain.Models.DBModels;
using Domain.Models.Response;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace DLL.Repository.Abstractions
{
    public class PriceRepository : IPriceRepository
    {
        private readonly AppDbContext _context;

        public PriceRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<OperationDetailsResponseModel> CreateAsync(PriceDBModel entity)
        {
            try
            {
                _context.Prices.Add(entity);
                await _context.SaveChangesAsync();
                return new OperationDetailsResponseModel() { IsError = false, Message = "Create success", Exception = null };
            }
            catch (Exception ex)
            {
                return new OperationDetailsResponseModel() { IsError = true, Message = "Create error", Exception = ex };
            }
        }
        public async Task<OperationDetailsResponseModel> DeleteAsync(int productId, int sellerId)
        {
            try
            {
                var entity = await _context.Prices
                                      .FirstOrDefaultAsync(cc => cc.ProductId == productId &&
                                                                 cc.SellerId == sellerId);
                if (entity == null)
                    return new OperationDetailsResponseModel { IsError = true, Message = "Entity not found" };

                _context.Prices.Remove(entity);
                await _context.SaveChangesAsync();
                return new OperationDetailsResponseModel() { IsError = false, Message = "Delete success", Exception = null };
            }
            catch (Exception ex)
            {
                return new OperationDetailsResponseModel() { IsError = true, Message = "Delete error", Exception = ex };
            }
        }

        public async Task<OperationDetailsResponseModel> UpdateAsync(PriceDBModel entity)
        {
            try
            {
                _context.Prices.Update(entity);
                await _context.SaveChangesAsync();
                return new OperationDetailsResponseModel() { IsError = false, Message = "Update success", Exception = null };
            }
            catch (Exception ex)
            {
                return new OperationDetailsResponseModel() { IsError = true, Message = "Update error", Exception = ex };
            }
        }

        public virtual async Task<IEnumerable<PriceDBModel>> GetFromConditionAsync(Expression<Func<PriceDBModel, bool>> condition) =>
            await _context.Prices.Where(condition).ToListAsync().ConfigureAwait(false);

        public IQueryable<PriceDBModel> GetQuery() => _context.Prices.AsQueryable();

        public async Task<IEnumerable<PriceDBModel>> ProcessQueryAsync(IQueryable<PriceDBModel> query) => 
            await query.ToListAsync();
    }
}
