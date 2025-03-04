using System.Linq.Expressions;
using AutoMapper;
using DLL.Repository;
using Domain.Models.DBModels;
using Domain.Models.Request.Seller;
using Domain.Models.Response;
using Domain.Models.Response.Seller;

namespace BLL.Services.SellerServices
{
    public class SellerService : ISellerService
    {
        private readonly IRepository<SellerDBModel, int> _repository;
        private readonly IMapper _mapper;

        public SellerService(IRepository<SellerDBModel, int> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<OperationResultModel<SellerDBModel>> CreateAsync(SellerCreateRequestModel request)
        {
            var model = _mapper.Map<SellerDBModel>(request);
            model.ApiKey = Guid.NewGuid().ToString();
            var repoResult = await _repository.CreateAsync(model);
            return repoResult.IsSuccess
                ? repoResult
                : OperationResultModel<SellerDBModel>.Failure(repoResult.ErrorMessage!, repoResult.Exception);
        }

        public async Task<OperationResultModel<SellerDBModel>> UpdateAsync(SellerUpdateRequestModel request)
        {
            var existingRecords = await _repository.GetFromConditionAsync(x => x.Id == request.Id);
            var existing = existingRecords.FirstOrDefault();
            if (existing == null)
            {
                return OperationResultModel<SellerDBModel>.Failure("Seller not found.");
            }

            _mapper.Map(request, existing);

            var repoResult = await _repository.UpdateAsync(existing);
            return repoResult.IsSuccess
                ? repoResult
                : OperationResultModel<SellerDBModel>.Failure(repoResult.ErrorMessage!, repoResult.Exception);
        }

        public async Task<OperationResultModel<bool>> DeleteAsync(int id)
        {
            var repoResult = await _repository.DeleteAsync(id);
            return repoResult.IsSuccess
                ? repoResult
                : OperationResultModel<bool>.Failure(repoResult.ErrorMessage!, repoResult.Exception);
        }

        public IQueryable<SellerDBModel> GetQuery()
        {
            return _repository.GetQuery();
        }

        public async Task<IEnumerable<SellerResponseModel>> GetFromConditionAsync(Expression<Func<SellerDBModel, bool>> condition)
        {
            var dbModels = await _repository.GetFromConditionAsync(condition);
            return _mapper.Map<IEnumerable<SellerResponseModel>>(dbModels);
        }

        public async Task<IEnumerable<SellerDBModel>> ProcessQueryAsync(IQueryable<SellerDBModel> query)
        {
            return await _repository.ProcessQueryAsync(query);
        }
    }
}
