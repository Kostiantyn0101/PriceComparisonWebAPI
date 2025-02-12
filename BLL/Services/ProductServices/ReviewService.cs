using System.Linq.Expressions;
using AutoMapper;
using DLL.Repository;
using Domain.Models.DBModels;
using Domain.Models.Request.Products;
using Domain.Models.Response;
using Domain.Models.Response.Products;

namespace BLL.Services.ProductServices
{
    public class ReviewService : IReviewService
    {
        private readonly IRepository<ReviewDBModel> _repository;
        private readonly IMapper _mapper;

        public ReviewService(IRepository<ReviewDBModel> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<OperationResultModel<bool>> CreateAsync(ReviewCreateRequestModel request)
        {
            var model = _mapper.Map<ReviewDBModel>(request);
            var repoResult = await _repository.CreateAsync(model);
            return !repoResult.IsError
                ? OperationResultModel<bool>.Success(true)
                : OperationResultModel<bool>.Failure(repoResult.Message, repoResult.Exception);
        }

        public async Task<OperationResultModel<bool>> UpdateAsync(ReviewUpdateRequestModel request)
        {
            var existingRecords = await _repository.GetFromConditionAsync(x => x.Id == request.Id);
            var existing = existingRecords.FirstOrDefault();
            if (existing == null)
            {
                return OperationResultModel<bool>.Failure("Review not found.");
            }

            existing.ReviewUrl = request.ReviewUrl;

            var repoResult = await _repository.UpdateAsync(existing);
            return !repoResult.IsError
                ? OperationResultModel<bool>.Success(true)
                : OperationResultModel<bool>.Failure(repoResult.Message, repoResult.Exception);

        }

        public async Task<OperationResultModel<bool>> DeleteAsync(int id)
        {
            var repoResult = await _repository.DeleteAsync(id);
            return !repoResult.IsError
                ? OperationResultModel<bool>.Success(true)
                : OperationResultModel<bool>.Failure(repoResult.Message, repoResult.Exception);
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
