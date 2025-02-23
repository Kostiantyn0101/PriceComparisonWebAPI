using System;
using Domain.Models.Response;

namespace BLL.Services.SellerServices
{
    public interface ISellerProductService
    {
        Task<OperationResultModel<string>> ProcessXmlAsync(Stream stream);
    }
}
