using Domain.Models.Configuration;
using PriceComparisonWebAPI.Filters;
using System.Text.Json.Serialization;

namespace PriceComparisonWebAPI.Infrastructure.DependencyInjection
{
    public static class AddCorsExtension
    {
        public static void AddCors(this WebApplicationBuilder builder)
        {
            var mvcAdminConfig = builder.Configuration
                .GetSection(MvcAdminConfiguration.Position)
                .Get<MvcAdminConfiguration>();

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowMVC", policyBuilder =>
                {
                    policyBuilder.WithOrigins(mvcAdminConfig?.AllowedOrigin)
                                 .AllowAnyHeader()
                                 .AllowAnyMethod()
                                 .AllowCredentials();
                });
            });
        }

    }
}

