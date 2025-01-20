using BLL.Services;
using BLL.Services.Auth;

namespace PriceComparisonWebAPI.Infrastructure.DependencyInjection
{
    public static class AddServicesExtension
    {
        public static void AddServices(this WebApplicationBuilder builder) {
            
            builder.Services.AddScoped<ICategoryService, CategoryService>();
            builder.Services.AddScoped<IAuthService, AuthService>();
        }
    }
}
