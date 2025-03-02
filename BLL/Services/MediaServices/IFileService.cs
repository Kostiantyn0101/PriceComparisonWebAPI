using Domain.Models.Response;
using Microsoft.AspNetCore.Http;

namespace BLL.Services.MediaServices
{
    public interface IFileService
    {
        Task<OperationResultModel<string>> SaveImageAsync(IFormFile file);
        Task<OperationResultModel<string>> DeleteImageAsync(string relativePath);
        Task<IFormFile> CreateFormFileFromUrlAsync(string imageUrl);
    }
}
