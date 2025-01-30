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
        private readonly FileStorageOptions _options;
        private readonly string _mediaPath;

        public FileService(IWebHostEnvironment env, IOptions<FileStorageOptions> options)
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

            var uniqueFileName = $"{Guid.NewGuid()}_{fileName}";
            var filePath = Path.Combine(directoryPath, uniqueFileName);

            await File.WriteAllBytesAsync(filePath, fileContent);

            return Path.Combine(relativePath, uniqueFileName).Replace("\\", "/");
        }

        public Task<string> SaveImageAsync(string fileName, byte[] fileContent) =>
            SaveFileAsync(_options.ImagesFolder, fileName, fileContent);

        public Task<string> SaveInstructionAsync(string fileName, byte[] fileContent) =>
            SaveFileAsync(_options.InstructionsFolder, fileName, fileContent);
    }
}
