using System.Linq.Expressions;
using AutoMapper;
using DLL.Repository;
using Domain.Models.DBModels;
using Domain.Models.Request.Products;
using Domain.Models.Response.Products;
using Domain.Models.Response;

namespace BLL.Services.ProductServices
{
    public class BaseProductService : IBaseProductService
    {
        private readonly IRepository<BaseProductDBModel, int> _repository;
        private readonly IMapper _mapper;

        public BaseProductService(IRepository<BaseProductDBModel, int> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<OperationResultModel<BaseProductDBModel>> CreateAsync(BaseProductCreateRequestModel request)
        {
            var model = _mapper.Map<BaseProductDBModel>(request);
            model.AddedToDatabase = DateTime.UtcNow;
            var repoResult = await _repository.CreateAsync(model);
            return repoResult.IsSuccess
                ? repoResult
                : OperationResultModel<BaseProductDBModel>.Failure(repoResult.ErrorMessage!, repoResult.Exception);
        }

        public async Task<OperationResultModel<BaseProductDBModel>> UpdateAsync(BaseProductUpdateRequestModel request)
        {
            var existingRecords = await _repository.GetFromConditionAsync(x => x.Id == request.Id);
            var existing = existingRecords.FirstOrDefault();
            if (existing == null)
            {
                return OperationResultModel<BaseProductDBModel>.Failure("BaseProduct record not found.");
            }

            _mapper.Map(request, existing);

            var repoResult = await _repository.UpdateAsync(existing);
            return repoResult.IsSuccess
                ? repoResult
                : OperationResultModel<BaseProductDBModel>.Failure(repoResult.ErrorMessage!, repoResult.Exception);
        }

        public async Task<OperationResultModel<bool>> DeleteAsync(int id)
        {
            var repoResult = await _repository.DeleteAsync(id);
            return repoResult.IsSuccess
                ? repoResult
                : OperationResultModel<bool>.Failure(repoResult.ErrorMessage!, repoResult.Exception);
        }

        public IQueryable<BaseProductDBModel> GetQuery()
        {
            return _repository.GetQuery();
        }

        public async Task<IEnumerable<BaseProductResponseModel>> GetFromConditionAsync(Expression<Func<BaseProductDBModel, bool>> condition)
        {
            var dbModels = await _repository.GetFromConditionAsync(condition);
            return _mapper.Map<IEnumerable<BaseProductResponseModel>>(dbModels);
        }

        public async Task<IEnumerable<BaseProductDBModel>> ProcessQueryAsync(IQueryable<BaseProductDBModel> query)
        {
            return await _repository.ProcessQueryAsync(query);
        }
    }
}
