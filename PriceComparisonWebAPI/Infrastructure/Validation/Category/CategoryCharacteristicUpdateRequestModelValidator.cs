using FluentValidation;
using PriceComparisonWebAPI.Controllers.Category;

namespace PriceComparisonWebAPI.Infrastructure.Validation.Category
{
    public class CategoryCharacteristicUpdateRequestModelValidator : AbstractValidator<CategoryCharacteristicUpdateRequestModel>
    {
        public CategoryCharacteristicUpdateRequestModelValidator()
        {
            RuleFor(x => x.OldCategoryId)
                           .GreaterThan(0).WithMessage("OldCategoryId must be greater than 0");

            RuleFor(x => x.OldCharacteristicId)
                .GreaterThan(0).WithMessage("OldCharacteristicId must be greater than 0");

            RuleFor(x => x.NewCategoryId)
                .GreaterThan(0).WithMessage("NewCategoryId must be greater than 0");

            RuleFor(x => x.NewCharacteristicId)
                .GreaterThan(0).WithMessage("NewCharacteristicId must be greater than 0");

            RuleFor(x => x)
                .Must(x => x.OldCategoryId != x.NewCategoryId || x.OldCharacteristicId != x.NewCharacteristicId)
                .WithMessage("At least one value (CategoryId or CharacteristicId) must be changed.");

        }
    }
}
