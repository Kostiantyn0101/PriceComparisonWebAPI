using Domain.Models.Request.Seller;
using FluentValidation;

namespace PriceComparisonWebAPI.Infrastructure.Validation.Seller
{
    public class AuctionClickRateUpdateRequestModelValidator : AbstractValidator<AuctionClickRateUpdateRequestModel>
    {
        public AuctionClickRateUpdateRequestModelValidator()
        {
            RuleFor(x => x.Id)
                .GreaterThan(0)
                .WithMessage("Id must be greater than 0.");

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
