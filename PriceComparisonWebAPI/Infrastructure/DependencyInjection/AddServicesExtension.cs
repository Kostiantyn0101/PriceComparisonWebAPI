using BLL.Services.Auth;
using BLL.Services.CategoryCharacteristicService;
using BLL.Services.CategoryService;
using BLL.Services.FeedbackAndReviewServices;
using BLL.Services.MediaServices;
using BLL.Services.PriceServices;
using BLL.Services.ProductService;
using BLL.Services.ProductServices;
using BLL.Services.SellerServices;

namespace PriceComparisonWebAPI.Infrastructure.DependencyInjection
{
    public static class AddServicesExtension
    {
        public static void AddServices(this WebApplicationBuilder builder) {
            
            builder.Services.AddScoped<IAuthService, AuthService>();
            builder.Services.AddScoped<ICategoryCharacteristicService, CategoryCharacteristicService>();
            builder.Services.AddScoped<IProductCharacteristicService, ProductCharacteristicService>();
            builder.Services.AddScoped<IProductSellerLinkService, ProductSellerLinkService>();
            builder.Services.AddScoped<IRelatedCategoryService, RelatedCategoryService>();
            builder.Services.AddScoped<ICharacteristicService, CharacteristicService>();
            builder.Services.AddScoped<IFeedbackImageService, FeedbackImageService>();
            builder.Services.AddScoped<IPriceHistoryService, PriceHistoryService>();
            builder.Services.AddScoped<IProductImageService, ProductImageService>();
            builder.Services.AddScoped<IProductVideoService, ProductVideoService>();
            builder.Services.AddScoped<IInstructionService, InstructionService>();
            builder.Services.AddScoped<ICategoryService, CategoryService>();
            builder.Services.AddScoped<IFeedbackService, FeedbackService>();
            builder.Services.AddScoped<IProductService, ProductService>();
            builder.Services.AddScoped<ISellerService, SellerService>();
            builder.Services.AddScoped<IReviewService, ReviewService>();
            builder.Services.AddScoped<IPriceService, PriceService>();
            builder.Services.AddScoped<IFileService, FileService>();
        }
    }
}
