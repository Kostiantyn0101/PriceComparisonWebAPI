using Domain.Models.Request.Seller;
using FluentValidation;

namespace PriceComparisonWebAPI.Infrastructure.Validation.Seller
{
    public class SellerProductdetailsRequestModelValidator : AbstractValidator<SellerProductDetailsRequestModel>
    {
        public SellerProductdetailsRequestModelValidator()
        {
            RuleFor(x => x.BaseProductId)
                   .GreaterThan(0).WithMessage("BaseProductId must be greater than 0.");

            RuleFor(x => x.ProductGroupId)
                   .GreaterThan(0).WithMessage("ProductGroupId must be greater than 0.");
        }
    }
}
