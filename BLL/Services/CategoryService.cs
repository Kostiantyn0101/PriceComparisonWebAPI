using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using DLL.Repository;
using Domain.Models.DBModels;
using Domain.Models.Response;

namespace BLL.Services
{
    public class CategoryService(IRepository<CategoryDBModel> repository) : ICategoryService
    {
        private readonly IRepository<CategoryDBModel> _repository = repository;

        public async Task<OperationDetailsResponseModel> CreateAsync(CategoryDBModel model)
        {
            try
            {
                return await _repository.CreateAsync(model);
            }
            catch (Exception ex)
            {
                return new OperationDetailsResponseModel { IsError = true, Message = "Fatal Error", Exception = ex };
            }
        }
        public async Task<OperationDetailsResponseModel> UpdateAsync(CategoryDBModel entity)
        {
            return await _repository.UpdateAsync(entity);
        }

        public async Task<OperationDetailsResponseModel> DeleteAsync(int id)
        {
            return await _repository.DeleteAsync(id);
        }
        public IQueryable<CategoryDBModel> GetQuery()
        {
            return _repository.GetQuery();
        }

        public async Task<IEnumerable<CategoryDBModel>> GetFromConditionAsync(Expression<Func<CategoryDBModel, bool>> condition)
        {
            return await _repository.GetFromConditionAsync(condition);
        }


        public async Task<IEnumerable<CategoryDBModel>> ProcessQueryAsync(IQueryable<CategoryDBModel> query)
        {
            return await _repository.ProcessQueryAsync(query);
        }

    }
}
