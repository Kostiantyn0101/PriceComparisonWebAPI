using Domain.Models.Request.Products;
using FluentValidation;

namespace PriceComparisonWebAPI.Infrastructure.Validation
{
    public class CharacteristicGroupRequestModelValidator : AbstractValidator<CharacteristicRequestModel>
    {
        public CharacteristicGroupRequestModelValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Title is required.")
                .MaximumLength(100).WithMessage("Title length must be less than 100 characters");
        }
    }
}
