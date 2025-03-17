using Domain.Models.Request.Products;
using FluentValidation;

namespace PriceComparisonWebAPI.Infrastructure.Validation.Product
{
    public class ProductGroupTypeCreateRequestValidator : AbstractValidator<ProductGroupTypeCreateRequestModel>
    {
        public ProductGroupTypeCreateRequestValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required.")
                .MaximumLength(100).WithMessage("Name should not exceed 100 characters.");
        }
    }
}
