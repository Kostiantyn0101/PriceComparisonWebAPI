using Microsoft.Extensions.FileProviders;
using PriceComparisonWebAPI.Infrastructure;
using PriceComparisonWebAPI.Infrastructure.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

ConfigurationService.ConfigureServices(builder);

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseFileStorageStaticFiles();

app.MapControllers();

app.Run();

