﻿using Domain.Models.Configuration;

namespace PriceComparisonWebAPI.Infrastructure.DependencyInjection
{
    public static class AddConfigurationExtension
    {
        public static void AddConfiguration(this WebApplicationBuilder builder)
        {
            builder.Configuration.SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            .AddJsonFile("appsettings.Secrets.json", optional: true, reloadOnChange: true);

            builder.Services.Configure<JwtConfiguration>(builder.Configuration.GetSection(JwtConfiguration.Position));
            builder.Services.Configure<FileStorageConfiguration>(builder.Configuration.GetSection(FileStorageConfiguration.Position));
            builder.Services.Configure<PopularProductsSettings>(builder.Configuration.GetSection(PopularProductsSettings.Position));
            builder.Services.Configure<SellerAccountConfiguration>(builder.Configuration.GetSection(SellerAccountConfiguration.Position));
            builder.Services.Configure<MvcAdminConfiguration>(builder.Configuration.GetSection(MvcAdminConfiguration.Position));
        }
    }
}
