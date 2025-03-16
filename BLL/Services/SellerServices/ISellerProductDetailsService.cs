using System;
using System.Linq.Expressions;
using Domain.Models.DBModels;
using Domain.Models.Request.Seller;
using Domain.Models.Response;
using Domain.Models.Response.Products;
using Domain.Models.Response.Seller;

namespace BLL.Services.SellerServices
{
    public interface ISellerProductDetailsService
    {
        Task<OperationResultModel<string>> ProcessXmlAsync(Stream stream);
        Task<IEnumerable<SellerProductDetailsResponseModel>> GetSellerProductDetailsAsync(int productId);
        Task<OperationResultModel<PaginatedResponse<SellerProductDetailsResponseModel>>> GetPaginatedSellerProductDetailsAsync(
                Expression<Func<SellerProductDetailsDBModel, bool>> condition, int page, int pageSize);
        Task<IEnumerable<SellerProductDetailsResponseModel>> GetSellerProductDetailsByProductGroupAsync(SellerProductDetailsRequestModel model);
    }
}
