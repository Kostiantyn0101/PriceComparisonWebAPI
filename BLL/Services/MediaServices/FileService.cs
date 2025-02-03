using Domain.Models.Configuration;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services.MediaServices
{
    public class FileService : IFileService
    {
        private readonly FileStorageConfiguration _options;
        IWebHostEnvironment _env;

        public FileService(IWebHostEnvironment env, IOptions<FileStorageConfiguration> options)
        {
            _options = options.Value;
            _env = env;
        }

        public async Task<string> SaveImageAsync(IFormFile file)
        {
            var fileGuid = Guid.NewGuid().ToString();
            var extension = Path.GetExtension(file.FileName);

            // Use two first characters of GUID for subdirectories creation
            var subDir1 = fileGuid.Substring(0, 2);
            var subDir2 = fileGuid.Substring(2, 2);
            var fileName = $"{fileGuid}{extension}";

            var fullDirectoryPath = Path.Combine(_env.WebRootPath, _options.ImagesFolder, subDir1, subDir2);
            var fullFilePath = Path.Combine(fullDirectoryPath, fileName);
            var relativeFilePath = Path.Combine(_options.ImagesFolder, subDir1, subDir2, fileName);

            // Create dir unless it exists
            Directory.CreateDirectory(fullDirectoryPath);

            using (var stream = new FileStream(fullFilePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            return $"/{relativeFilePath.Replace("\\", "/")}";
        }
    }
}
