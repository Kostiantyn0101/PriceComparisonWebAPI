using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace BLL.Services.MediaServices
{
    public interface IFileService
    {
        Task<string> SaveImageAsync(IFormFile file);
    }
}
