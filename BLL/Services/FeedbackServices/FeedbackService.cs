﻿using System.Linq.Expressions;
using AutoMapper;
using DLL.Repository;
using Domain.Models.DBModels;
using Domain.Models.Request.Feedback;
using Domain.Models.Response;
using Domain.Models.Response.Feedback;
using Microsoft.EntityFrameworkCore;

namespace BLL.Services.FeedbackAndReviewServices
{
    public class FeedbackService : IFeedbackService
    {
        private readonly IRepository<FeedbackDBModel, int> _repository;
        private readonly IMapper _mapper;
        public FeedbackService(IRepository<FeedbackDBModel, int> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<OperationResultModel<FeedbackDBModel>> CreateAsync(FeedbackCreateRequestModel request)
        {
            var model = _mapper.Map<FeedbackDBModel>(request);

            model.CreatedAt = DateTime.UtcNow;
            var result = await _repository.CreateAsync(model);
            return result.IsSuccess
                ? result
                : OperationResultModel<FeedbackDBModel>.Failure(result.ErrorMessage!, result.Exception);
        }

        public async Task<OperationResultModel<FeedbackDBModel>> UpdateAsync(FeedbackUpdateRequestModel request)
        {
            var existingFeedbacks = await _repository.GetFromConditionAsync(x => x.Id == request.Id);
            var existingFeedback = existingFeedbacks.FirstOrDefault();
            if (existingFeedback == null)
            {
                return OperationResultModel<FeedbackDBModel>.Failure("Feedback not found");
            }

            existingFeedback.FeedbackText = request.FeedbackText;
            existingFeedback.Rating = request.Rating;

            var result = await _repository.UpdateAsync(existingFeedback);
            return result.IsSuccess
                ? result
                : OperationResultModel<FeedbackDBModel>.Failure(result.ErrorMessage!, result.Exception);
        }

        public async Task<OperationResultModel<bool>> DeleteAsync(int id)
        {
            var result = await _repository.DeleteAsync(id);
            return result.IsSuccess
                ? result
                : OperationResultModel<bool>.Failure(result.ErrorMessage!, result.Exception);
        }

        public async Task<IEnumerable<FeedbackResponseModel>> GetFromConditionAsync(Expression<Func<FeedbackDBModel, bool>> condition)
        {
            var dbModels = await _repository.GetFromConditionAsync(condition);
            return _mapper.Map<IEnumerable<FeedbackResponseModel>>(dbModels);
        }

        public IQueryable<FeedbackDBModel> GetQuery()
        {
            return _repository.GetQuery();
        }

        public async Task<IEnumerable<FeedbackDBModel>> ProcessQueryAsync(IQueryable<FeedbackDBModel> query)
        {
            return await _repository.ProcessQueryAsync(query);
        }

        public async Task<OperationResultModel<PaginatedResponse<FeedbackResponseModel>>> GetPaginatedFeedbacksAsync(
                        Expression<Func<FeedbackDBModel, bool>> condition, int page, int pageSize)
        {
            var query = _repository.GetQuery()
                .Include(f => f.User)
                .Include(f => f.FeedbackImages)
                .Where(condition);

            var totalItems = await query.CountAsync();

            var feedbacks = await query
                .OrderByDescending(f => f.CreatedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            var mappedFeedbacks = _mapper.Map<IEnumerable<FeedbackResponseModel>>(feedbacks);

            var response = new PaginatedResponse<FeedbackResponseModel>
            {
                Data = mappedFeedbacks,
                Page = page,
                PageSize = pageSize,
                TotalItems = totalItems
            };

            return OperationResultModel<PaginatedResponse<FeedbackResponseModel>>.Success(response);
        }
    }
}
