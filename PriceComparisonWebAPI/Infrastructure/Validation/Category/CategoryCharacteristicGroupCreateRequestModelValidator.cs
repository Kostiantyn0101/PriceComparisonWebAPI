using Domain.Models.Request.Categories;
using FluentValidation;

namespace PriceComparisonWebAPI.Infrastructure.Validation.Category
{
    public class CategoryCharacteristicGroupCreateRequestModelValidator : AbstractValidator<CategoryCharacteristicGroupCreateRequestModel>
    {
        public CategoryCharacteristicGroupCreateRequestModelValidator()
        {
            RuleFor(x => x.CategoryId)
                .GreaterThan(0).WithMessage("Category ID must be greater than 0.");

            RuleFor(x => x.CharacteristicGroupId)
                .GreaterThan(0).WithMessage("Characteristic Group ID must be greater than 0.");

            RuleFor(x => x.GroupDisplayOrder)
                 .GreaterThan(0).WithMessage("Group display order must greater than 0.");
        }
    }
}
