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
        Task<IEnumerable<CategoryDBModel>> GetFromConditionAsync(Expression<Func<CategoryDBModel, bool>> condition);
        IQueryable<CategoryDBModel> GetQuery();
        Task<IEnumerable<CategoryDBModel>> ProcessQueryAsync(IQueryable<CategoryDBModel> query);
        Task<OperationDetailsResponseModel> UpdateAsync(int id, CategoryDBModel entity);
        Task<OperationDetailsResponseModel> CreateAsync(CategoryDBModel model);
    }
}
