using Domain.Models.Request.Feedback;
using FluentValidation;

namespace PriceComparisonWebAPI.Infrastructure.Validation
{
    public class FeedbackImageDeleteRequestModelValidator : AbstractValidator<FeedbackImageDeleteRequestModel>
    {
        public FeedbackImageDeleteRequestModelValidator()
        {
            RuleFor(x => x.FeedbackImageIds)
                .NotNull().WithMessage("FeedbackImageIds must be provided")
                .Must(ids => ids != null && ids.Any())
                .WithMessage("At least one FeedbackImageId must be provided");
        }
    }
}
