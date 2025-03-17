using Domain.Models.Request.Products;
using FluentValidation;

namespace PriceComparisonWebAPI.Infrastructure.Validation.Product
{
    public class ProductGroupTypeUpdateRequestValidator : AbstractValidator<ProductGroupTypeUpdateRequestModel>
    {
        public ProductGroupTypeUpdateRequestValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("Id must be greater than 0");

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required.")
                .MaximumLength(100).WithMessage("Name should not exceed 100 characters.");
        }
    }
}
