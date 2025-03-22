using Domain.Models.Request.Feedback;
using FluentValidation;

namespace PriceComparisonWebAPI.Infrastructure.Validation.Feedback
{
    public class FeedbackCreateRequestModelValidator : AbstractValidator<FeedbackCreateRequestModel>
    {
        public FeedbackCreateRequestModelValidator()
        {
            RuleFor(x => x.UserId)
                .GreaterThan(0).WithMessage("UserId must be greater than 0.");

            RuleFor(x => x.BaseProductId)
                .GreaterThan(0).WithMessage("BaseProductId must be greater than 0.");

            RuleFor(x => x.FeedbackText)
                .NotEmpty().WithMessage("FeedbackText is required.");

            RuleFor(x => x.Rating)
                .InclusiveBetween(1, 5).WithMessage("Rating have to be between 1 and 5.");
        }
    }
}
