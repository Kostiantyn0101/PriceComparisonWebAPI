using DLL.Repository;

namespace PriceComparisonWebAPI.Infrastructure.DependencyInjection
{
    public static class AddRepositoriesExtension
    {
        public static void AddRepositories(this WebApplicationBuilder builder)
        {
            builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
        }
    }
}
