using Domain.Models.Request.Feedback;
using FluentValidation;

namespace PriceComparisonWebAPI.Infrastructure.Validation.Feedback
{
    public class FeedbackImageCreateRequestModelValidator : AbstractValidator<FeedbackImageCreateRequestModel>
    {
        public FeedbackImageCreateRequestModelValidator()
        {
            RuleFor(x => x.FeedbackId)
                .GreaterThan(0).WithMessage("FeedbackId must be greater than 0");
            RuleFor(x => x.Images)
                .NotNull().WithMessage("Images must be provided")
                .Must(images => images != null && images.Any())
                .WithMessage("At least one image must be provided");
        }
    }
}
