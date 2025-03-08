using Domain.Models.Request.Filters;
using FluentValidation;

namespace PriceComparisonWebAPI.Infrastructure.Validation.Filters
{
    public class FilterCriterionCreateRequestModelValidator : AbstractValidator<FilterCriterionCreateRequestModel>
    {
        public FilterCriterionCreateRequestModelValidator()
        {
            RuleFor(x => x.FilterId)
                .GreaterThan(0).WithMessage("FilterId must be greater than 0.");

            RuleFor(x => x.CharacteristicId)
                .GreaterThan(0).WithMessage("CharacteristicId must be greater than 0.");

            RuleFor(x => x.Operator)
                .NotEmpty().WithMessage("Operator is required.");

            RuleFor(x => x.Value)
                .GreaterThanOrEqualTo(0).WithMessage("Value must be non-negative.");
        }
    }
}
