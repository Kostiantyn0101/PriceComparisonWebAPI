using DLL.Context;
using Domain.Models.DBModels;
using Microsoft.AspNetCore.Identity;

namespace PriceComparisonWebAPI.Infrastructure.DependencyInjection
{
    public static class AddIdentityExtensions
    {
        public static void AddIdentity(this WebApplicationBuilder builder)
        {
            builder.Services.AddIdentity<ApplicationUserDBModel, IdentityRole<int>>(options =>
            {
                options.SignIn.RequireConfirmedEmail = true;
            }
           ).AddEntityFrameworkStores<AppDbContext>().AddDefaultTokenProviders();
        }
    }
}
