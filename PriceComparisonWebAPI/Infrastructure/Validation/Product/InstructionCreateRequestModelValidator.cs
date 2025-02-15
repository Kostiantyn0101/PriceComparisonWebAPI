using Domain.Models.Request.Products;
using FluentValidation;

namespace PriceComparisonWebAPI.Infrastructure.Validation.Product
{
    public class InstructionCreateRequestModelValidator : AbstractValidator<InstructionCreateRequestModel>
    {
        public InstructionCreateRequestModelValidator()
        {
            RuleFor(x => x.ProductId)
                .GreaterThan(0).WithMessage("ProductId must be greater than 0.");
            RuleFor(x => x.InstructionUrl)
                .NotEmpty().WithMessage("InstructionUrl is required.")
                .Must(url => Uri.TryCreate(url, UriKind.Absolute, out _))
                .WithMessage("InstructionUrl must be a valid URL address.");
        }
    }
}
