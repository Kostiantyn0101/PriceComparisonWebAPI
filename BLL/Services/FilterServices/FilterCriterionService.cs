using AutoMapper;
using DLL.Repository;
using Domain.Models.DBModels;
using Domain.Models.Request.Filters;
using Domain.Models.Response;
using Domain.Models.Response.Filters;
using System.Linq.Expressions;

namespace BLL.Services.FilterServices
{
    public class FilterCriterionService : IFilterCriterionService
    {
        private readonly IRepository<FilterCriterionDBModel, int> _repository;
        private readonly IMapper _mapper;

        public FilterCriterionService(IRepository<FilterCriterionDBModel, int> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<OperationResultModel<FilterCriterionDBModel>> CreateAsync(FilterCriterionCreateRequestModel request)
        {
            var model = _mapper.Map<FilterCriterionDBModel>(request);
            var repoResult = await _repository.CreateAsync(model);
            return repoResult.IsSuccess
                ? repoResult
                : OperationResultModel<FilterCriterionDBModel>.Failure(repoResult.ErrorMessage!, repoResult.Exception);
        }

        public async Task<OperationResultModel<FilterCriterionDBModel>> UpdateAsync(FilterCriterionUpdateRequestModel request)
        {
            var existingRecords = await _repository.GetFromConditionAsync(x => x.Id == request.Id);
            var existing = existingRecords.FirstOrDefault();
            if (existing == null)
            {
                return OperationResultModel<FilterCriterionDBModel>.Failure("Filter criterion not found.");
            }

            _mapper.Map(request, existing);
            var repoResult = await _repository.UpdateAsync(existing);
            return repoResult.IsSuccess
                ? repoResult
                : OperationResultModel<FilterCriterionDBModel>.Failure(repoResult.ErrorMessage!, repoResult.Exception);
        }

        public async Task<OperationResultModel<bool>> DeleteAsync(int id)
        {
            return await _repository.DeleteAsync(id);
        }

        public IQueryable<FilterCriterionDBModel> GetQuery()
        {
            return _repository.GetQuery();
        }

        public async Task<IEnumerable<FilterCriterionResponseModel>> GetFromConditionAsync(Expression<Func<FilterCriterionDBModel, bool>> condition)
        {
            var dbModels = await _repository.GetFromConditionAsync(condition);
            return _mapper.Map<IEnumerable<FilterCriterionResponseModel>>(dbModels);
        }

        public async Task<IEnumerable<FilterCriterionDBModel>> ProcessQueryAsync(IQueryable<FilterCriterionDBModel> query)
        {
            return await _repository.ProcessQueryAsync(query);
        }
    }
}
