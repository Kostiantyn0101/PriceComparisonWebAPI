using System.Linq.Expressions;
using AutoMapper;
using DLL.Repository;
using Domain.Models.DBModels;
using Domain.Models.Request.Products;
using Domain.Models.Response.Products;
using Domain.Models.Response;

namespace BLL.Services.ProductServices
{
    public class ProductGroupTypeService : IProductGroupTypeService
    {
        private readonly IRepository<ProductGroupTypeDBModel, int> _repository;
        private readonly IMapper _mapper;

        public ProductGroupTypeService(IRepository<ProductGroupTypeDBModel, int> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<OperationResultModel<ProductGroupTypeDBModel>> CreateAsync(ProductGroupTypeCreateRequestModel request)
        {
            var model = _mapper.Map<ProductGroupTypeDBModel>(request);
            var newResult = await _repository.CreateAsync(model);
            return newResult;
        }

        public async Task<OperationResultModel<ProductGroupTypeDBModel>> UpdateAsync(ProductGroupTypeUpdateRequestModel request)
        {
            var existingRecords = await _repository.GetFromConditionAsync(x => x.Id == request.Id);
            var existing = existingRecords.FirstOrDefault();
            if (existing == null)
            {
                return OperationResultModel<ProductGroupTypeDBModel>.Failure("Record ProductGroupType not found.");
            }

            _mapper.Map(request, existing);
            var repoResult = await _repository.UpdateAsync(existing);
            return repoResult.IsSuccess
                ? repoResult
                : OperationResultModel<ProductGroupTypeDBModel>.Failure(repoResult.ErrorMessage, repoResult.Exception);
        }

        public async Task<OperationResultModel<bool>> DeleteAsync(int id)
        {
            var repoResult = await _repository.DeleteAsync(id);
            return repoResult.IsSuccess
                ? repoResult
                : OperationResultModel<bool>.Failure(repoResult.ErrorMessage, repoResult.Exception);
        }

        public IQueryable<ProductGroupTypeDBModel> GetQuery()
        {
            return _repository.GetQuery();
        }

        public async Task<IEnumerable<ProductGroupTypeResponseModel>> GetFromConditionAsync(Expression<Func<ProductGroupTypeDBModel, bool>> condition)
        {
            var dbModels = await _repository.GetFromConditionAsync(condition);
            return _mapper.Map<IEnumerable<ProductGroupTypeResponseModel>>(dbModels);
        }

        public async Task<IEnumerable<ProductGroupTypeDBModel>> ProcessQueryAsync(IQueryable<ProductGroupTypeDBModel> query)
        {
            return await _repository.ProcessQueryAsync(query);
        }
    }
}
