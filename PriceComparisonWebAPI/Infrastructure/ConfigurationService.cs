using DLL.Context;
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

            }
            catch (Exception ex) {
                throw;
            }
        }
    }
}
