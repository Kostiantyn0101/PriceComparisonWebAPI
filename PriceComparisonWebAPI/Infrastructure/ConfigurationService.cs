using BLL.Services;
using DLL.Context;
using DLL.Repository;
using DLL.Repository.Abstractions;
using Domain.Models.DBModels;
using Microsoft.EntityFrameworkCore;
using PriceComparisonWebAPI.Infrastructure.DependencyInjection;

namespace PriceComparisonWebAPI.Infrastructure
{
    public class ConfigurationService
    {
        private WebApplicationBuilder builder;
        public ConfigurationService(WebApplicationBuilder builder)
        {
            this.builder = builder;
        }

        public void ConfigureService()
        {
            try
            {
                builder.AddConfiguration();
                builder.ConfigureJsonOptions();
                builder.AddDbContext();
                builder.AddIdentity();
                builder.AddRepositories();
                builder.AddServices();
                builder.AddAuth();
                builder.AddSwagger();

            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
