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
            
            RuleFor(x => x.Name)
             .MaximumLength(255).WithMessage("Product group name length must be less than 255 characters")
             .When(x => x.Name != null);
        }
    }

}
