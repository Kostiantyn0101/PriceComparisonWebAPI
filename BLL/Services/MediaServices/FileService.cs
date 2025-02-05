using Domain.Models.Configuration;
using Domain.Models.Response;
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

        public async Task<OperationResultModel<string>> DeleteImageAsync(string relativePath)
        {
            try
            {
                var path = Path.Combine(_env.WebRootPath, relativePath.TrimStart('/'));

                if (File.Exists(path))
                {
                    await Task.Run(() => File.Delete(path));
                    return OperationResultModel<string>.Success();
                }
                else
                {
                    // todo logger
                    return OperationResultModel<string>.Failure($"File {relativePath} does not exist. Deletion unpossible");
                }
            }
            catch (Exception ex)
            {
                return OperationResultModel<string>.Failure($"File wasn't deleted", ex);
            }
        }

        public async Task<OperationResultModel<string>> SaveImageAsync(IFormFile file)
        {
            try
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

                return OperationResultModel<string>.Success($"/{relativeFilePath.Replace("\\", "/")}");
            }
            catch (Exception ex)
            {
                //throw new Exception(ex.Message);
                return OperationResultModel<string>.Failure($"File wasn't saved", ex);
            }
        }
    }
}
