using System.Linq.Expressions;
using AutoMapper;
using DLL.Repository;
using Domain.Models.DBModels;
using Domain.Models.Request.Products;
using Domain.Models.Response.Products;
using Domain.Models.Response;

namespace BLL.Services.ProductServices
{
    public class ProductGroupService : IProductGroupService
    {
        private readonly IRepository<ProductGroupDBModel, int> _repository;
        private readonly IMapper _mapper;

        public ProductGroupService(IRepository<ProductGroupDBModel, int> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<OperationResultModel<ProductGroupDBModel>> CreateAsync(ProductGroupCreateRequestModel request)
        {
            var model = _mapper.Map<ProductGroupDBModel>(request);
            var repoResult = await _repository.CreateAsync(model);
            return repoResult.IsSuccess
                ? repoResult
                : OperationResultModel<ProductGroupDBModel>.Failure(repoResult.ErrorMessage, repoResult.Exception);
        }

        public async Task<OperationResultModel<ProductGroupDBModel>> UpdateAsync(ProductGroupUpdateRequestModel request)
        {
            var existingRecords = await _repository.GetFromConditionAsync(x => x.Id == request.Id);
            var existing = existingRecords.FirstOrDefault();
            if (existing == null)
            {
                return OperationResultModel<ProductGroupDBModel>.Failure("ProductGroup record not found.");
            }

            _mapper.Map(request, existing);
            var repoResult = await _repository.UpdateAsync(existing);
            return repoResult.IsSuccess
                ? repoResult
                : OperationResultModel<ProductGroupDBModel>.Failure(repoResult.ErrorMessage, repoResult.Exception);
        }

        public async Task<OperationResultModel<bool>> DeleteAsync(int id)
        {
            var repoResult = await _repository.DeleteAsync(id);
            return repoResult.IsSuccess
                ? repoResult
                : OperationResultModel<bool>.Failure(repoResult.ErrorMessage, repoResult.Exception);
        }

        public IQueryable<ProductGroupDBModel> GetQuery()
        {
            return _repository.GetQuery();
        }

        public async Task<IEnumerable<ProductGroupResponseModel>> GetFromConditionAsync(Expression<Func<ProductGroupDBModel, bool>> condition)
        {
            var dbModels = await _repository.GetFromConditionAsync(condition);
            return _mapper.Map<IEnumerable<ProductGroupResponseModel>>(dbModels);
        }

        public async Task<IEnumerable<ProductGroupDBModel>> ProcessQueryAsync(IQueryable<ProductGroupDBModel> query)
        {
            return await _repository.ProcessQueryAsync(query);
        }
    }

}
