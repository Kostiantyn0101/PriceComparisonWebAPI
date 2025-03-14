using Domain.Models.Request.Products;
using FluentValidation;

namespace PriceComparisonWebAPI.Infrastructure.Validation.Product
{
    public class ColorUpdateRequestModelValidator : AbstractValidator<ColorUpdateRequestModel>
    {
        public ColorUpdateRequestModelValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("Color id must be greater than 0.");

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Color name is required.")
                .MaximumLength(100).WithMessage("Color name must be less than 100 characters");

            RuleFor(x => x.HexCode)
                .NotEmpty().WithMessage("Hex code is required.")
                .Matches("^#(?:[0-9a-fA-F]{3}){1,2}$").WithMessage("Hex code is invalid.");

            RuleFor(x => x.GradientCode)
                .MaximumLength(100).WithMessage("Gradient code must be less than 100 characters")
                .When(x => !string.IsNullOrEmpty(x.GradientCode));
        }
    }
}
