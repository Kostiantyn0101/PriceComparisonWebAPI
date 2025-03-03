using System;
using Domain.Models.DBModels;
using Domain.Models.Response;
using Domain.Models.Response.Seller;

namespace BLL.Services.SellerServices
{
    public interface ISellerProductDetailsService
    {
        Task<OperationResultModel<string>> ProcessXmlAsync(Stream stream);
        Task<IEnumerable<SellerProductDetailsResponseModel>> GetSellerProductDetailsAsync(int productId);
    }
}
