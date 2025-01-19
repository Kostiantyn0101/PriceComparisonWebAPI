using BLL.Services;
using DLL.Context;
using DLL.Repository;
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
                builder.Configuration.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

                //var connectionString = builder.Configuration["ConnectionStrings:PriceComparisonDB"];

                //builder.Services.AddDbContext<AppDbContext>(op => op.UseSqlServer(connectionString));

                //builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
                //builder.Services.AddScoped<ICategoryService, CategoryService>();


                builder.AddDbContext();
                builder.AddIdentity();
                builder.AddRepositories();
                builder.AddServices();

            }
            catch (Exception ex) {
                throw;  
            }
        }
    }
}
