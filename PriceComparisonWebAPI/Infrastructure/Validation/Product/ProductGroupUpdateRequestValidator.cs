namespace PriceComparisonWebAPI.Infrastructure.Validation.Product
{
    using Domain.Models.Request.Products;
    using FluentValidation;

    public class ProductGroupUpdateRequestValidator : AbstractValidator<ProductGroupUpdateRequestModel>
    {
        public ProductGroupUpdateRequestValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0).WithMessage("Id must be provided and greater than 0.");
            RuleFor(x => x.ProductId)
                .GreaterThan(0).WithMessage("ProductId must be greater than 0.");
            RuleFor(x => x.ProductGroupId)
                .NotEmpty().WithMessage("ProductGroupId is required.");
        }
    }

}
