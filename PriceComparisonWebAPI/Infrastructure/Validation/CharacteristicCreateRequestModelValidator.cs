using Domain.Models.Request.Products;
using FluentValidation;

namespace PriceComparisonWebAPI.Infrastructure.Validation
{
    public class CharacteristicCreateRequestModelValidator : AbstractValidator<CharacteristicRequestModel>
    {
        public CharacteristicCreateRequestModelValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Title is required.")
                .MaximumLength(100).WithMessage("Title length must be less than 100 characters");

            RuleFor(x => x.DataType)
                .NotEmpty().WithMessage("Data type is required.");

            RuleFor(x => x.Unit)
                .MaximumLength(50)
                .When(x => !string.IsNullOrEmpty(x.Unit))
                .WithMessage("Unit length must be less than 50 characters");
        
            RuleFor(x => x.CharacteristicGroupId)
                .GreaterThan(0).WithMessage("Characteristic group id must be greater than 0");

            RuleFor(x => x.DisplayOrder)
                .GreaterThan(0).WithMessage("Display order must be greater than 0");
        }
    }
}
