namespace PriceComparisonWebAPI.Infrastructure.Validation.Product
{
    using Domain.Models.Request.Products;
    using FluentValidation;

    public class ProductGroupCreateRequestValidator : AbstractValidator<ProductGroupCreateRequestModel>
    {
        public ProductGroupCreateRequestValidator()
        {
            RuleFor(x => x.NewProductId)
                .GreaterThan(0).WithMessage("NewProductId must be greater than 0.");
            RuleFor(x => x.ExistingProductId)
                .GreaterThan(0).WithMessage("ExistingProductId must be greater than 0.");
        }
    }

}
