using System.Linq.Expressions;
using AutoMapper;
using DLL.Repository;
using Domain.Models.DBModels;
using Domain.Models.Request.Products;
using Domain.Models.Response;
using Domain.Models.Response.Products;

namespace BLL.Services.ProductService
{
    public class ProductVideoService : IProductVideoService
    {
        private readonly IRepository<ProductVideoDBModel> _repository;
        private readonly IMapper _mapper;


        public ProductVideoService(IRepository<ProductVideoDBModel> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<OperationResultModel<bool>> CreateAsync(ProductVideoCreateRequestModel request)
        {
            var model = _mapper.Map<ProductVideoDBModel>(request);
            var result = await _repository.CreateAsync(model);
            return !result.IsError
                ? OperationResultModel<bool>.Success(true)
                : OperationResultModel<bool>.Failure(result.Message, result.Exception);
        }

        public async Task<OperationResultModel<bool>> UpdateAsync(ProductVideoUpdateRequestModel request)
        {
            var existingRecords = await _repository.GetFromConditionAsync(x => x.Id == request.Id);
            var existing = existingRecords.FirstOrDefault();
            if (existing == null)
            {
                return OperationResultModel<bool>.Failure("Product video not found.");
            }

            _mapper.Map(request, existing);

            var result = await _repository.UpdateAsync(existing);
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

        public IQueryable<ProductVideoDBModel> GetQuery()
        {
            return _repository.GetQuery();
        }

        public async Task<IEnumerable<ProductVideoResponseModel>> GetFromConditionAsync(Expression<Func<ProductVideoDBModel, bool>> condition)
        {
            var dbModels = await _repository.GetFromConditionAsync(condition);
            return _mapper.Map<IEnumerable<ProductVideoResponseModel>>(dbModels);
        }

        public async Task<IEnumerable<ProductVideoDBModel>> ProcessQueryAsync(IQueryable<ProductVideoDBModel> query)
        {
            return await _repository.ProcessQueryAsync(query);
        }
    }
}
