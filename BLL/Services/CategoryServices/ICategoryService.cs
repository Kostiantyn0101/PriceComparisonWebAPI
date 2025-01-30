using System.Linq.Expressions;
using Domain.Models.DBModels;
using Domain.Models.Response;
using Microsoft.AspNetCore.Http;

namespace BLL.Services.CategoryService
{
    public interface ICategoryService
    {
        Task<OperationDetailsResponseModel> CreateAsync(CategoryDBModel model);
        Task<OperationDetailsResponseModel> UpdateAsync(CategoryDBModel entity);
        Task<OperationDetailsResponseModel> DeleteAsync(int id);
        IQueryable<CategoryDBModel> GetQuery();
        Task<IEnumerable<CategoryDBModel>> GetFromConditionAsync(Expression<Func<CategoryDBModel, bool>> condition);
        Task<IEnumerable<CategoryDBModel>> ProcessQueryAsync(IQueryable<CategoryDBModel> query);
        Task<OperationDetailsResponseModel> UploadCategoryMediaAsync(int categoryId, string mediaType, IFormFile file);

    }
}
