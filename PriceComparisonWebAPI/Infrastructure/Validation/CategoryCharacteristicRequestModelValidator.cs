using Domain.Models.Request.Categories;
using FluentValidation;

namespace PriceComparisonWebAPI.Infrastructure.Validation
{
    public class CategoryCharacteristicRequestModelValidator : AbstractValidator<CategoryCharacteristicRequestModel>
    {
        public CategoryCharacteristicRequestModelValidator()
        {
            RuleFor(x => x.CategoryId)
                .GreaterThan(0).WithMessage("Category id must be greater than 0");

            RuleFor(x => x.CharacteristicIds)
                .NotEmpty().WithMessage("Characteristic Ids list is required");

            RuleForEach(x => x.CharacteristicIds)
                .GreaterThan(0).WithMessage("Each characteristic id must be greater than 0");
        }
    }

}
