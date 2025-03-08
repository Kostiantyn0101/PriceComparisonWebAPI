using AutoMapper;
using DLL.Repository;
using Domain.Models.DBModels;
using Domain.Models.Request.Filters;
using Domain.Models.Response;
using Domain.Models.Response.Filters;
using System.Linq.Expressions;

namespace BLL.Services.FilterServices
{
    public class FilterService : IFilterService
    {
        private readonly IRepository<FilterDBModel, int> _repository;
        private readonly IMapper _mapper;

        public FilterService(IRepository<FilterDBModel, int> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<OperationResultModel<FilterDBModel>> CreateAsync(FilterCreateRequestModel request)
        {
            var model = _mapper.Map<FilterDBModel>(request);
            var repoResult = await _repository.CreateAsync(model);
            return repoResult.IsSuccess
                ? repoResult
                : OperationResultModel<FilterDBModel>.Failure(repoResult.ErrorMessage!, repoResult.Exception);
        }

        public async Task<OperationResultModel<FilterDBModel>> UpdateAsync(FilterUpdateRequestModel request)
        {
            var existingRecords = await _repository.GetFromConditionAsync(x => x.Id == request.Id);
            var existing = existingRecords.FirstOrDefault();
            if (existing == null)
            {
                return OperationResultModel<FilterDBModel>.Failure("Filter not found.");
            }

            _mapper.Map(request, existing);
            var repoResult = await _repository.UpdateAsync(existing);
            return repoResult.IsSuccess
                ? repoResult
                : OperationResultModel<FilterDBModel>.Failure(repoResult.ErrorMessage!, repoResult.Exception);
        }

        public async Task<OperationResultModel<bool>> DeleteAsync(int id)
        {
            return await _repository.DeleteAsync(id);
        }

        public IQueryable<FilterDBModel> GetQuery()
        {
            return _repository.GetQuery();
        }

        public async Task<IEnumerable<FilterResponseModel>> GetFromConditionAsync(Expression<Func<FilterDBModel, bool>> condition)
        {
            var dbModels = await _repository.GetFromConditionAsync(condition);
            return _mapper.Map<IEnumerable<FilterResponseModel>>(dbModels);
        }

        public async Task<IEnumerable<FilterDBModel>> ProcessQueryAsync(IQueryable<FilterDBModel> query)
        {
            return await _repository.ProcessQueryAsync(query);
        }
    }
}
