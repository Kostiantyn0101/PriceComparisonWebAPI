using Domain.Models.Request.Seller;
using FluentValidation;

namespace PriceComparisonWebAPI.Infrastructure.Validation.Seller
{
    public class AuctionClickRateCreateRequestModelValidator : AbstractValidator<AuctionClickRateCreateRequestModel>
    {
        public AuctionClickRateCreateRequestModelValidator()
        {
            RuleFor(x => x.CategoryId)
                .GreaterThan(0)
                .WithMessage("CategoryId must be greater than 0.");

            RuleFor(x => x.SellerId)
                .GreaterThan(0)
                .WithMessage("SellerId must be greater than 0.");

            RuleFor(x => x.ClickRate)
                .GreaterThan(0)
                .WithMessage("Click rate must be greater than 0.");
        }
    }
}
