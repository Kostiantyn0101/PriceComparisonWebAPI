using Domain.Models.Request.Products;
using FluentValidation;

namespace PriceComparisonWebAPI.Infrastructure.Validation
{
    public class CharacteristicRequestModelValidator : AbstractValidator<CharacteristicRequestModel>
    {
        public CharacteristicRequestModelValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("Characteristic id must be greater than 0");

            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Title is required.")
                .MaximumLength(100).WithMessage("Title length must be less than 100 characters");

            RuleFor(x => x.DataType)
                .NotEmpty().WithMessage("Data type is required.");

            RuleFor(x => x.Unit)
                .MaximumLength(50)
                .When(x => !string.IsNullOrEmpty(x.Unit))
                .WithMessage("Unit length must be less than 50 characters");
        }
    }
}
