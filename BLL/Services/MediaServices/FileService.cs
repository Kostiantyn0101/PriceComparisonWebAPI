using Domain.Models.Configuration;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services.MediaServices
{
    public class FileService : IFileService
    {
        private readonly FileStorageConfiguration _options;
        private readonly string _mediaPath;

        public FileService(IWebHostEnvironment env, IOptions<FileStorageConfiguration> options)
        {
            _options = options.Value;
            _mediaPath = Path.Combine(env.ContentRootPath, _options.MediaFolder);
        }

        public async Task<string> SaveFileAsync(string relativePath, string fileName, byte[] fileContent)
        {
            var directoryPath = Path.Combine(_mediaPath, relativePath);
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            string extension = Path.GetExtension(fileName);
            var uniqueFileName = $"{Guid.NewGuid()}{extension}";
            var filePath = Path.Combine(directoryPath, uniqueFileName);

            await File.WriteAllBytesAsync(filePath, fileContent);

            return Path.Combine(relativePath, uniqueFileName).Replace("\\", "/");
        }

        public async Task<string> SaveImageAsync(string fileName, byte[] fileContent)
        {
            long maxSizeInBytes = _options.ImageSizeMB * 1024 * 1024;

            if (fileContent.Length > maxSizeInBytes)
            {
                throw new InvalidOperationException($"File size exceeds the allowed limit of {_options.ImageSizeMB} MB.");
            }

            return await SaveFileAsync(_options.ImagesFolder, fileName, fileContent);
        }

        public Task<string> SaveInstructionAsync(string fileName, byte[] fileContent) =>
            SaveFileAsync(_options.InstructionsFolder, fileName, fileContent);

    }
}
