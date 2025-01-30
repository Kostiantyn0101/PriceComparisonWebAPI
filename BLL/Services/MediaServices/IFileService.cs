using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Services.MediaServices
{
    public interface IFileService
    {
        Task<string> SaveFileAsync(string relativePath, string fileName, byte[] fileContent);
        Task<string> SaveImageAsync(string fileName, byte[] fileContent);
        Task<string> SaveInstructionAsync(string fileName, byte[] fileContent);

    }
}
