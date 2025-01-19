using BLL.Services;
using DLL.Context;
using DLL.Repository;
using Domain.Models.DBModels;
using Microsoft.EntityFrameworkCore;

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
                builder.Configuration.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

                var connectionString = builder.Configuration["ConnectionStrings:PriceComparisonDB"];

                builder.Services.AddDbContext<AppDbContext>(op => op.UseSqlServer(connectionString));

                builder.Services.AddScoped<CategoryRepository>();
                builder.Services.AddScoped<ICategoryService, CategoryService>();

            }
            catch (Exception ex) {
                throw;
            }
        }
    }
}
