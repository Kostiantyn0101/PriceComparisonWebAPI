using Domain.Models.Response;

namespace DLL.Repository
{
    public interface IRepositoryDelete<TEntity>
    {
        Task<OperationDetailsResponseModel> DeleteAsync(int id);
    }
}
