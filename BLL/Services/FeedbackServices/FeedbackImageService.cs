using System.Linq.Expressions;
using AutoMapper;
using BLL.Services.MediaServices;
using DLL.Repository;
using Domain.Models.DBModels;
using Domain.Models.Request.Feedback;
using Domain.Models.Response;
using Domain.Models.Response.Feedback;

namespace BLL.Services.FeedbackAndReviewServices
{
    public class FeedbackImageService : IFeedbackImageService
    {

        private readonly IRepository<FeedbackImageDBModel> _repository;
        private readonly IFileService _fileService;
        private readonly IMapper _mapper;

        public FeedbackImageService(IRepository<FeedbackImageDBModel> repository, IFileService fileService, IMapper mapper)
        {
            _repository = repository;
            _fileService = fileService;
            _mapper = mapper;
        }

        public async Task<OperationResultModel<bool>> CreateAsync(FeedbackImageCreateRequestModel request)
        {
            if (request.Images == null || !request.Images.Any())
            {
                return OperationResultModel<bool>.Failure("No images provided.");
            }

            int successCount = 0;
            var errors = new List<string>();

            foreach (var image in request.Images)
            {
                //need logic for separete from pic of products
                var saveResult = await _fileService.SaveImageAsync(image);
                if (!saveResult.IsSuccess)
                {
                    errors.Add(saveResult.ErrorMessage);
                    continue;
                }

                var feedbackImage = new FeedbackImageDBModel
                {
                    FeedbackId = request.FeedbackId,
                    ImageUrl = saveResult.Data
                };

                var repoResult = await _repository.CreateAsync(feedbackImage);
                if (!repoResult.IsError)
                {
                    successCount++;
                }
                else
                {
                    errors.Add(repoResult.Message);
                }
            }

            if (successCount == 0)
            {
                return OperationResultModel<bool>.Failure("No image was successfully saved: " + string.Join("; ", errors));
            }
            return OperationResultModel<bool>.Success(true);
        }


        public async Task<OperationResultModel<bool>> DeleteAsync(FeedbackImageDeleteRequestModel request)
        {
            int successCount = 0;
            var errors = new List<string>();

            foreach (var id in request.FeedbackImageIds)
            {
                var records = await _repository.GetFromConditionAsync(x => x.Id == id);
                var feedbackImage = records.FirstOrDefault();
                if (feedbackImage != null)
                {
                    var deleteFileResult = await _fileService.DeleteImageAsync(feedbackImage.ImageUrl);
                    var repoResult = await _repository.DeleteAsync(id);
                    if (!repoResult.IsError)
                    {
                        successCount++;
                    }
                    else
                    {
                        errors.Add(repoResult.Message);
                    }
                }
                else
                {
                    errors.Add($"Image with id {id} not found.");
                }
            }

            if (successCount == 0)
            {
                return OperationResultModel<bool>.Failure("No image was successfully deleted: " + string.Join("; ", errors));
            }
            return OperationResultModel<bool>.Success(true);
        }
        public async Task<OperationDetailsResponseModel> UpdateAsync(FeedbackImageDBModel entity)
        {
            return await _repository.UpdateAsync(entity);
        }

        public IQueryable<FeedbackImageDBModel> GetQuery()
        {
            return _repository.GetQuery();
        }

        public async Task<IEnumerable<FeedbackImageResponseModel>> GetFromConditionAsync(Expression<Func<FeedbackImageDBModel, bool>> condition)
        {
            var dbModels = await _repository.GetFromConditionAsync(condition);
            return _mapper.Map<IEnumerable<FeedbackImageResponseModel>>(dbModels);
        }

        public async Task<IEnumerable<FeedbackImageDBModel>> ProcessQueryAsync(IQueryable<FeedbackImageDBModel> query)
        {
            return await _repository.ProcessQueryAsync(query);
        }
    }
}
