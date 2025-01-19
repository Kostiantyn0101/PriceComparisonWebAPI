using Domain.Models.Response;

namespace DLL.Repository
{
    public interface IRepositoryUpdate<TEntity> : IRepository<TEntity>
    {
        Task<OperationDetailsResponseModel> UpdateAsync(int id, TEntity entityNew);
    }
}
