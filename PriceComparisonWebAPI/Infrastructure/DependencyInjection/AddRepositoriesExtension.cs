using DLL.Repository;
using DLL.Repository.Abstractions;

namespace PriceComparisonWebAPI.Infrastructure.DependencyInjection
{
    public static class AddRepositoriesExtension
    {
        public static void AddRepositories(this WebApplicationBuilder builder)
        {
            builder.Services.AddScoped(typeof(IRepository<,>), typeof(Repository<,>));
            builder.Services.AddScoped(typeof(ICompositeKeyRepository<,,>), typeof(CompositeKeyRepository<,,>));

            //for refactor
            builder.Services.AddScoped<ICategoryCharacteristicRepository, CategoryCharacteristicRepository>();
            builder.Services.AddScoped<IProductCharacteristicRepository, ProductCharacteristicRepository>();
            builder.Services.AddScoped<IRelatedCategoryRepository, RelatedCategoryRepository>();
            //builder.Services.AddScoped<ISellerProductDetailsRepository, SellerProductDetailsRepository>();
        }
    }
}
