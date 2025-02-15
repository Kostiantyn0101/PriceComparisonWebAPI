using Domain.Models.Request.Categories;
using FluentValidation;

namespace PriceComparisonWebAPI.Infrastructure.Validation.Category
{
    public class CategoryUpdateRequestModelValidator : AbstractValidator<CategoryUpdateRequestModel>
    {
        public CategoryUpdateRequestModelValidator()
        {
            RuleFor(x => x.Title)
               .NotEmpty().WithMessage("Category title is required.")
               .MaximumLength(100).WithMessage("Category title must not exceed 100 characters.");

            RuleFor(x => x.ParentCategoryId)
                .GreaterThan(1).When(x => x.ParentCategoryId.HasValue)
                .WithMessage("Parent category ID must be 1 or greater.");

            RuleFor(x => x.NewImage)
                .ImageFileRules();

            RuleFor(x => x.NewIcon)
                .ImageFileRules();
        }
    }
}
