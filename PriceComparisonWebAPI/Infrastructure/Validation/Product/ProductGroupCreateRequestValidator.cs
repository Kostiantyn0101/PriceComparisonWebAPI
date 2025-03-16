namespace PriceComparisonWebAPI.Infrastructure.Validation.Product
{
    using Domain.Models.Request.Products;
    using FluentValidation;

    public class ProductGroupCreateRequestValidator : AbstractValidator<ProductGroupCreateRequestModel>
    {
        public ProductGroupCreateRequestValidator()
        {
            RuleFor(x => x.Name)
              .MaximumLength(255).WithMessage("Product group name length must be less than 255 characters")
              .When(x => x.Name != null);
        }
    }

}
