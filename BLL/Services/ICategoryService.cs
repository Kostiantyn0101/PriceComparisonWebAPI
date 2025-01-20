using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Domain.Models.DBModels;
using Domain.Models.Response;

namespace BLL.Services
{
    public interface ICategoryService
    {
        Task<OperationDetailsResponseModel> CreateAsync(CategoryDBModel model);
        Task<OperationDetailsResponseModel> UpdateAsync(CategoryDBModel entity);
        Task<OperationDetailsResponseModel> DeleteAsync(int id);
        IQueryable<CategoryDBModel> GetQuery();
        Task<IEnumerable<CategoryDBModel>> GetFromConditionAsync(Expression<Func<CategoryDBModel, bool>> condition);
        Task<IEnumerable<CategoryDBModel>> ProcessQueryAsync(IQueryable<CategoryDBModel> query);
    }
}
