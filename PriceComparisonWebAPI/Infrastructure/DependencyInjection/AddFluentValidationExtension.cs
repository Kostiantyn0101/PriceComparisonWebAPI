using FluentValidation;
using FluentValidation.AspNetCore;

namespace PriceComparisonWebAPI.Infrastructure.DependencyInjection
{
    public static class AddFluentValidationExtension
    {
        public static void AddFluentValidation(this WebApplicationBuilder builder)
        {
            builder.Services
                .AddFluentValidationAutoValidation()
                .AddFluentValidationClientsideAdapters();
            
            builder.Services.AddValidatorsFromAssemblyContaining<Program>();
        }
    }
}
