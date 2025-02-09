using System.Linq.Expressions;
using AutoMapper;
using DLL.Repository;
using Domain.Models.DBModels;
using Domain.Models.Request.Products;
using Domain.Models.Response;
using Domain.Models.Response.Products;

namespace BLL.Services.ProductService
{
    public class ProductService : IProductService
    {
        private readonly IRepository<ProductDBModel> _repository;
        private readonly IMapper _mapper;

        public ProductService(IRepository<ProductDBModel> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<OperationDetailsResponseModel> CreateAsync(ProductRequestModel model)
        {
            return await _repository.CreateAsync(_mapper.Map<ProductDBModel>(model));
        }

        public async Task<OperationDetailsResponseModel> UpdateAsync(ProductRequestModel entity)
        {
            return await _repository.UpdateAsync(_mapper.Map<ProductDBModel>(entity));
        }

        public async Task<OperationDetailsResponseModel> DeleteAsync(int id)
        {
            return await _repository.DeleteAsync(id);
        }

        public IQueryable<ProductDBModel> GetQuery()
        {
            return _repository.GetQuery();
        }

        public async Task<IEnumerable<ProductResponseModel>> GetFromConditionAsync(Expression<Func<ProductDBModel, bool>> condition)
        {
            var dbModels = await _repository.GetFromConditionAsync(condition);
            return _mapper.Map<IEnumerable<ProductResponseModel>>(dbModels);
        }

        public async Task<IEnumerable<ProductDBModel>> ProcessQueryAsync(IQueryable<ProductDBModel> query)
        {
            return await _repository.ProcessQueryAsync(query);
        }
    }
}
