using Domain.Models.Request.Products;
using FluentValidation;

namespace PriceComparisonWebAPI.Infrastructure.Validation.Product
{
    public class InstructionUpdateRequestModelValidator : AbstractValidator<InstructionUpdateRequestModel>
    {
        public InstructionUpdateRequestModelValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("Id must be greater than 0.");

            RuleFor(x => x.BaseProductId)
                .GreaterThan(0).WithMessage("BaseProductId must be greater than 0");

            RuleFor(x => x.InstructionUrl)
                .NotEmpty().WithMessage("InstructionUrl is required.")
                .Must(url => Uri.TryCreate(url, UriKind.Absolute, out _))
                .WithMessage("InstructionUrl must be a valid URL address.");
        }
    }
}
