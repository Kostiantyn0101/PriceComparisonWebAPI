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

        public async Task<IFormFile> CreateFormFileFromUrlAsync(string imageUrl)
        {
            if (string.IsNullOrEmpty(imageUrl))
            {
                throw new ArgumentException("Image URL cannot be null or empty", nameof(imageUrl));
            }

            try
            {
                using var httpClient = new HttpClient();
                // Set a timeout for the entire HTTP operation
                httpClient.Timeout = TimeSpan.FromSeconds(30);

                // Create a cancellation token for the download
                using var cts = new CancellationTokenSource(TimeSpan.FromSeconds(30));

                // Download the file with the cancellation token
                using var response = await httpClient.GetAsync(imageUrl, HttpCompletionOption.ResponseHeadersRead, cts.Token);
                response.EnsureSuccessStatusCode();

                var contentType = response.Content.Headers.ContentType?.ToString() ?? "image/webp";
                var fileName = GetFileNameFromUrl(imageUrl) ?? "image.webp";

                // Create memory stream (without setting timeouts)
                var memoryStream = new MemoryStream();

                // Copy with the same cancellation token
                await response.Content.CopyToAsync(memoryStream, cts.Token);

                if (memoryStream.Length == 0)
                {
                    throw new InvalidOperationException("Downloaded stream has zero length");
                }

                memoryStream.Position = 0;

                // Create the FormFile
                var formFile = new FormFile(
                    baseStream: memoryStream,
                    baseStreamOffset: 0,
                    length: memoryStream.Length,
                    name: "file",
                    fileName: fileName)
                {
                    ContentType = contentType
                };

                return formFile;
            }
            catch (OperationCanceledException)
            {
                throw new TimeoutException($"Timeout occurred while downloading image from: {imageUrl}");
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to create FormFile from URL: {imageUrl}", ex);
            }
        }

        private string GetFileNameFromUrl(string url)
        {
            var uri = new Uri(url);
            var fileName = Path.GetFileName(uri.LocalPath);
            return !string.IsNullOrEmpty(fileName) ? fileName : "file";
        }
    }
}
