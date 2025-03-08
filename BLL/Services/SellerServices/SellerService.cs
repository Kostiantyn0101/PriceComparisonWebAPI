using System.Linq.Expressions;
using AutoMapper;
using BLL.Services.MediaServices;
using DLL.Repository;
using Domain.Models.DBModels;
using Domain.Models.Primitives;
using Domain.Models.Request.Seller;
using Domain.Models.Response;
using Domain.Models.Response.Seller;

namespace BLL.Services.SellerServices
{
    public class SellerService : ISellerService
    {
        private readonly IRepository<SellerDBModel, int> _repository;
        private readonly IFileService _fileService;
        private readonly IMapper _mapper;

        public SellerService(IRepository<SellerDBModel, int> repository, IFileService fileService, IMapper mapper)
        {
            _repository = repository;
            _fileService = fileService;
            _mapper = mapper;
        }

        public async Task<OperationResultModel<SellerDBModel>> CreateAsync(SellerCreateRequestModel request)
        {
            var model = _mapper.Map<SellerDBModel>(request);
            model.ApiKey = Guid.NewGuid().ToString();

            if (request.LogoImage != null)
            {
                var result = await _fileService.SaveImageAsync(request.LogoImage);

                if (result.IsSuccess)
                {
                    model.LogoImageUrl = result.Data;
                }
                else
                {
                    return OperationResultModel<SellerDBModel>.Failure("Image save error");
                }
            }

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

            if ((request.NewLogoImage != null || request.DeleteCurrentLogoImage) && existing.LogoImageUrl != null)
            {
                var deleteResult = await _fileService.DeleteImageAsync(existing.LogoImageUrl);
                if (deleteResult.IsSuccess)
                {
                    existing.LogoImageUrl = null;
                }
            }

            if (request.NewLogoImage != null)
            {
                var saveResult = await _fileService.SaveImageAsync(request.NewLogoImage);
                if (saveResult.IsSuccess)
                {
                    existing.LogoImageUrl = saveResult.Data;
                }
                else
                {
                    return OperationResultModel<SellerDBModel>.Failure("Image save error");
                }
            }

            var repoResult = await _repository.UpdateAsync(existing);
            return repoResult.IsSuccess
                ? repoResult
                : OperationResultModel<SellerDBModel>.Failure(repoResult.ErrorMessage!, repoResult.Exception);
        }

        public async Task<OperationResultModel<bool>> DeleteAsync(int id)
        {
            var dbModel = (await _repository.GetFromConditionAsync(x => x.Id == id)).FirstOrDefault();

            if (dbModel != null && dbModel.LogoImageUrl != null)
            {
                await _fileService.DeleteImageAsync(dbModel.LogoImageUrl);
            }

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
