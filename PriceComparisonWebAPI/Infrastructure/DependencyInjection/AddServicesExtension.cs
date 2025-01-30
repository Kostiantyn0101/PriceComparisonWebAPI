using BLL.Services.Auth;
using BLL.Services.CategoryService;
using BLL.Services.MediaServices;
using BLL.Services.ProductServices;
using Domain.Models.Configuration;

namespace PriceComparisonWebAPI.Infrastructure.DependencyInjection
{
    public static class AddServicesExtension
    {
        public static void AddServices(this WebApplicationBuilder builder) {
            
            builder.Services.AddScoped<ICategoryService, CategoryService>();
            builder.Services.AddScoped<ICharacteristicService, CharacteristicService>();
            builder.Services.AddScoped<ICategoryCharacteristicService, CategoryCharacteristicService>();
            builder.Services.AddScoped<IAuthService, AuthService>();
            builder.Services.AddScoped<IFileService, FileService>();
        }
    }
}
