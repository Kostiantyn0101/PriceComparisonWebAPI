using Domain.Models.Configuration;
using Microsoft.Extensions.FileProviders;


namespace PriceComparisonWebAPI.Infrastructure.DependencyInjection
{
    public static class AddFileStorageExtension
    {
        public static void AddFileStorage(this WebApplicationBuilder builder)
        {
            var fileStorageConfig = builder.Configuration
                .GetSection(FileStorageConfiguration.Position)
                .Get<FileStorageConfiguration>();

            CreateMediaDirectories(builder.Environment.ContentRootPath, fileStorageConfig);

            builder.Services.AddSingleton(fileStorageConfig);
        }

        private static void CreateMediaDirectories(string rootPath, FileStorageConfiguration config)
        {
            var directories = new[]
            {
                Path.Combine(rootPath, config.MediaFolder, config.ImagesFolder),
                Path.Combine(rootPath, config.MediaFolder, config.InstructionsFolder)
            };

            foreach (var path in directories)
            {
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
            }
        }

        public static void UseFileStorageStaticFiles(this WebApplication app)
        {
            var fileStorageConfig = app.Services.GetRequiredService<FileStorageConfiguration>();

            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(
                    Path.Combine(app.Environment.ContentRootPath, fileStorageConfig.MediaFolder, fileStorageConfig.ImagesFolder)),
                RequestPath = "/images"
            });
        }
    }
}