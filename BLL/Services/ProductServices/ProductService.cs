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

        public async Task<OperationResultModel<bool>> CreateAsync(ProductRequestModel model)
        {
            var mapped = _mapper.Map<ProductDBModel>(model);
            var result = await _repository.CreateAsync(mapped);
            return !result.IsError
                ? OperationResultModel<bool>.Success(true)
                : OperationResultModel<bool>.Failure(result.Message, result.Exception);
        }

        public async Task<OperationResultModel<bool>> UpdateAsync(ProductRequestModel model)
        {
            var mapped = _mapper.Map<ProductDBModel>(model);
            var result = await _repository.UpdateAsync(mapped);
            return !result.IsError
                ? OperationResultModel<bool>.Success(true)
                : OperationResultModel<bool>.Failure(result.Message, result.Exception);
        }

        public async Task<OperationResultModel<bool>> DeleteAsync(int id)
        {
            var result = await _repository.DeleteAsync(id);
            return !result.IsError
                ? OperationResultModel<bool>.Success(true)
                : OperationResultModel<bool>.Failure(result.Message, result.Exception);
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
