using DLL.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace PriceComparisonWebAPI.Infrastructure.DependencyInjection
{
    public static class AddDBbContextExtension
    {
        public static void AddDbContext(this WebApplicationBuilder builder)
        {
            var conStr = builder.Configuration["ConnectionStrings:PriceComparisonDB"]; //dbcontext

            builder.Services.AddDbContext<AppDbContext>(op => op.UseSqlServer(conStr)
                                                                    .EnableSensitiveDataLogging(false)
                                                                    .ConfigureWarnings(warnings =>
                                                                    {
                                                                        warnings.Ignore(RelationalEventId.CommandExecuted);
                                                                    }));

            builder.Services.AddScoped<DbContext, AppDbContext>(); //db context
        }
    }
}
