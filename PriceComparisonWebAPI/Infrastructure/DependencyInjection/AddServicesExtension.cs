using BLL.Services;

namespace PriceComparisonWebAPI.Infrastructure.DependencyInjection
{
    public static class AddServicesExtension
    {
        public static void AddServices(this WebApplicationBuilder builder) {
            builder.Services.AddScoped<ICategoryService, CategoryService>();
        }
    }
}
