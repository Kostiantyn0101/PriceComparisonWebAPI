using Domain.Models.Configuration;
using Microsoft.Extensions.FileProviders;

namespace PriceComparisonWebAPI.Infrastructure.DependencyInjection
{
    public static class AddFileStorageExtension
    {
        public static void UseFileStorageStaticFiles(this WebApplication app)
        {
            var imageDirName = app.Configuration.GetSection(FileStorageConfiguration.Position).GetSection("ImagesFolder").Value;
            
            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(
                    Path.Combine(app.Environment.WebRootPath, imageDirName)),
                RequestPath = "/images"
            });
        }
    }
}