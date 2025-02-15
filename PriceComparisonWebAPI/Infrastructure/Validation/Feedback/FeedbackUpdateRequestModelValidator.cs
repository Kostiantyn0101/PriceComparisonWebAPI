using Domain.Models.Request.Feedback;
using FluentValidation;

namespace PriceComparisonWebAPI.Infrastructure.Validation.Feedback
{
    public class FeedbackUpdateRequestModelValidator : AbstractValidator<FeedbackUpdateRequestModel>
    {
        public FeedbackUpdateRequestModelValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("Id must be greater than 0.");
            RuleFor(x => x.FeedbackText)
                .NotEmpty().WithMessage("FeedbackText is required.");
            RuleFor(x => x.Rating)
                .InclusiveBetween(1, 5).WithMessage("Rating have to be between 1 and 5.");
        }
    }
}
