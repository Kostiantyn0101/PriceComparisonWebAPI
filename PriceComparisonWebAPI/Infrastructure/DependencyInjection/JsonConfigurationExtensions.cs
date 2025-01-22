using DLL.Context;
using Domain.Models.DBModels;
using Microsoft.AspNetCore.Identity;

namespace PriceComparisonWebAPI.Infrastructure.DependencyInjection
{
    public static class JsonConfigurationExtensions
    {
        public static void ConfigureJsonOptions(this WebApplicationBuilder builder)
        {
            builder.Services.AddControllers()
                                .AddJsonOptions(options =>
                                {
                                    options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve;
                                });
        }
    }
}
