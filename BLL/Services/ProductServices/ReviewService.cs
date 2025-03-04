using AutoMapper;
using DLL.Repository;
using Domain.Models.DBModels;
using Domain.Models.Request.Products;
using Domain.Models.Response;
using Domain.Models.Response.Products;
using System.Linq.Expressions;

namespace BLL.Services.ProductServices
{
    public class ReviewService : IReviewService
    {
        private readonly IRepository<ReviewDBModel, int> _repository;
        private readonly IMapper _mapper;

        public ReviewService(IRepository<ReviewDBModel, int> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<OperationResultModel<ReviewDBModel>> CreateAsync(ReviewCreateRequestModel request)
        {
            var model = _mapper.Map<ReviewDBModel>(request);
            var repoResult = await _repository.CreateAsync(model);
            return repoResult.IsSuccess
                ? repoResult
                : OperationResultModel<ReviewDBModel>.Failure(repoResult.ErrorMessage!, repoResult.Exception);
        }

        public async Task<OperationResultModel<ReviewDBModel>> UpdateAsync(ReviewUpdateRequestModel request)
        {
            var existingRecords = await _repository.GetFromConditionAsync(x => x.Id == request.Id);
            var existing = existingRecords.FirstOrDefault();
            if (existing == null)
            {
                return OperationResultModel<ReviewDBModel>.Failure("Review not found.");
            }

            existing.ReviewUrl = request.ReviewUrl;

            var repoResult = await _repository.UpdateAsync(existing);
            return repoResult.IsSuccess
                ? repoResult
                : OperationResultModel<ReviewDBModel>.Failure(repoResult.ErrorMessage!, repoResult.Exception);

        }

        public async Task<OperationResultModel<bool>> DeleteAsync(int id)
        {
            var repoResult = await _repository.DeleteAsync(id);
            return repoResult.IsSuccess
                ? OperationResultModel<bool>.Success(true)
                : OperationResultModel<bool>.Failure(repoResult.ErrorMessage!, repoResult.Exception);
        }

        public IQueryable<ReviewDBModel> GetQuery()
        {
            return _repository.GetQuery();
        }

        public async Task<IEnumerable<ReviewResponseModel>> GetFromConditionAsync(Expression<Func<ReviewDBModel, bool>> condition)
        {
            var dbModels = await _repository.GetFromConditionAsync(condition);
            return _mapper.Map<IEnumerable<ReviewResponseModel>>(dbModels);
        }

        public async Task<IEnumerable<ReviewDBModel>> ProcessQueryAsync(IQueryable<ReviewDBModel> query)
        {
            return await _repository.ProcessQueryAsync(query);
        }
    }
}
