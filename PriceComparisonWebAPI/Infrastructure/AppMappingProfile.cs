using AutoMapper;
using Domain.Models.DBModels;
using Domain.Models.Request.Categories;
using Domain.Models.Request.Feedback;
using Domain.Models.Request.Products;
using Domain.Models.Response.Categories;
using Domain.Models.Response.Feedback;
using Domain.Models.Response.Products;
using PriceComparisonWebAPI.Infrastructure.MapperResolvers;

namespace PriceComparisonWebAPI.Infrastructure
{
    public class AppMappingProfile : Profile
    {
        public AppMappingProfile()
        {
            
            // CATEGORIES
            CreateMap<CategoryDBModel, CategoryResponseModel>()
                .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom<CategoryImageUrlResolver>())
                .ForMember(dest => dest.IconUrl, opt => opt.MapFrom<CategoryIconUrlResolver>());

            CreateMap<CategoryCreateRequestModel, CategoryDBModel>();
            CreateMap<CategoryUpdateRequestModel, CategoryDBModel>();

            CreateMap<CharacteristicDBModel, CharacteristicResponseModel>();
            CreateMap<CharacteristicCreateRequestModel, CharacteristicDBModel>();
            CreateMap<CharacteristicRequestModel, CharacteristicDBModel>();

            CreateMap<RelatedCategoryDBModel, RelatedCategoryResponseModel>();
            CreateMap<RelatedCategoryRequestModel, RelatedCategoryDBModel>();

            CreateMap<CategoryCharacteristicDBModel, CategoryCharacteristicResponseModel>()
                .ForMember(dest => dest.CharacteristicTitle, opt => opt.MapFrom(src => src.Characteristic.Title))
                .ForMember(dest => dest.CharacteristicDataType, opt => opt.MapFrom(src => src.Characteristic.DataType))
                .ForMember(dest => dest.CharacteristicUnit, opt => opt.MapFrom(src => src.Characteristic.Unit));
            CreateMap<CategoryCharacteristicRequestModel, CategoryCharacteristicDBModel>();

            
            // PRODUCTS
            CreateMap<ProductDBModel, ProductResponseModel>();
            CreateMap<ProductRequestModel, ProductDBModel>();

            // todo: find desision about nullable type
            CreateMap<ProductImageDBModel, ProductImageResponseModel>()
                .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom<ProductImageUrlResolver>());

            CreateMap<ProductCharacteristicDBModel, ProductCharacteristicResponseModel>()
               .ForMember(dest => dest.CharacteristicTitle, opt => opt.MapFrom(src => src.Characteristic.Title))
               .ForMember(dest => dest.CharacteristicDataType, opt => opt.MapFrom(src => src.Characteristic.DataType))
               .ForMember(dest => dest.CharacteristicUnit, opt => opt.MapFrom(src => src.Characteristic.Unit));
            CreateMap<ProductCharacteristicValueUpdateModel, ProductCharacteristicDBModel>();

            CreateMap<ProductVideoDBModel, ProductVideoResponseModel>();
            CreateMap<ProductVideoCreateRequestModel, ProductVideoDBModel>();
            CreateMap<ProductVideoUpdateRequestModel, ProductVideoDBModel>();

            CreateMap<InstructionDBModel, InstructionResponseModel>();
            CreateMap<InstructionCreateRequestModel, InstructionDBModel>();
            CreateMap<InstructionUpdateRequestModel, InstructionDBModel>();

            CreateMap<FeedbackDBModel, FeedbackResponseModel>();
            CreateMap<FeedbackCreateRequestModel, FeedbackDBModel>();
            CreateMap<FeedbackUpdateRequestModel, FeedbackDBModel>()
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.User, opt => opt.Ignore())
                .ForMember(dest => dest.Product, opt => opt.Ignore())
                .ForMember(dest => dest.FeedbackImages, opt => opt.Ignore());

            CreateMap<FeedbackImageDBModel, FeedbackImageResponseModel>();

            CreateMap<ReviewDBModel, ReviewResponseModel>();
            CreateMap<ReviewCreateRequestModel, ReviewDBModel>();
            CreateMap<ReviewUpdateRequestModel, ReviewDBModel>();
        }
    }
}
