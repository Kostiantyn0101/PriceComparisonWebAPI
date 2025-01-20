using BLL.Services;
using DLL.Context;
using DLL.Repository;
using DLL.Repository.Abstractions;
using Domain.Models.DBModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace PriceComparisonWebAPI.Infrastructure.DependencyInjection
{
    public static class AddRepositoriesExtension
    {
        public static void AddRepositories(this WebApplicationBuilder builder)
        {
            builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

            //builder.Services.AddScoped<IRepository<CategoryDBModel>, Repository<CategoryDBModel>>();
        }
    }
}
