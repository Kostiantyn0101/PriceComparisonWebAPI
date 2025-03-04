namespace PriceComparisonWebAPI.Infrastructure.Validation.Product
{
    using Domain.Models.Request.Products;
    using FluentValidation;

    public class ProductGroupCreateRequestValidator : AbstractValidator<ProductGroupCreateRequestModel>
    {
        public ProductGroupCreateRequestValidator()
        {
            RuleFor(x => x.ProductId)
                .GreaterThan(0).WithMessage("ProductId must be greater than 0.");
            RuleFor(x => x.ProductGroupId)
                .NotEmpty().WithMessage("ProductGroupId is required.");
        }
    }

}
