using AutoMapper;
using Domain.Models.DBModels;
using Domain.Models.Request.Categories;
using Domain.Models.Request.Feedback;
using Domain.Models.Request.Products;
using Domain.Models.Request.Seller;
using Domain.Models.Response.Categories;
using Domain.Models.Response.Feedback;
using Domain.Models.Response.Products;
using Domain.Models.Response.Seller;
using PriceComparisonWebAPI.Infrastructure.MapperResolvers;

namespace PriceComparisonWebAPI.Infrastructure
{
    public class AppMappingProfile : Profile
    {
        public AppMappingProfile()
        {
            // CHARACTERISTICS
            CreateMap<CharacteristicDBModel, CharacteristicResponseModel>();
            CreateMap<CharacteristicCreateRequestModel, CharacteristicDBModel>();
            CreateMap<CharacteristicRequestModel, CharacteristicDBModel>()
                    .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
            
            CreateMap<CharacteristicGroupDBModel, CharacteristicGroupResponseModel>();
            CreateMap<CharacteristicGroupRequestModel, CharacteristicGroupDBModel>();


            // CATEGORIES
            CreateMap<CategoryDBModel, CategoryResponseModel>()
                .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom<CategoryImageUrlResolver>())
                .ForMember(dest => dest.IconUrl, opt => opt.MapFrom<CategoryIconUrlResolver>());

            CreateMap<CategoryCreateRequestModel, CategoryDBModel>();
            CreateMap<CategoryUpdateRequestModel, CategoryDBModel>();

            CreateMap<RelatedCategoryDBModel, RelatedCategoryResponseModel>();
            CreateMap<RelatedCategoryRequestModel, RelatedCategoryDBModel>();

            CreateMap<CategoryCharacteristicDBModel, CategoryCharacteristicResponseModel>()
                .ForMember(dest => dest.CharacteristicTitle, opt => opt.MapFrom(src => src.Characteristic.Title))
                .ForMember(dest => dest.CharacteristicDataType, opt => opt.MapFrom(src => src.Characteristic.DataType))
                .ForMember(dest => dest.CharacteristicUnit, opt => opt.MapFrom(src => src.Characteristic.Unit));
            CreateMap<CategoryCharacteristicRequestModel, CategoryCharacteristicDBModel>();

            
            // PRODUCTS
            CreateMap<ProductDBModel, ProductResponseModel>();
            CreateMap<ProductRequestModel, ProductDBModel>()
                    .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            // todo: find desision about nullable type
            CreateMap<ProductImageDBModel, ProductImageResponseModel>()
                .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom<ProductImageUrlResolver>());
                
            CreateMap<FeedbackImageDBModel, FeedbackImageResponseModel>()
                .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom<FeedbackImageUrlResolver>());

            CreateMap<ProductCharacteristicDBModel, ProductCharacteristicResponseModel>()
               .ForMember(dest => dest.CharacteristicTitle, opt => opt.MapFrom(src => src.Characteristic.Title))
               .ForMember(dest => dest.CharacteristicDataType, opt => opt.MapFrom(src => src.Characteristic.DataType))
               .ForMember(dest => dest.CharacteristicUnit, opt => opt.MapFrom(src => src.Characteristic.Unit));
            CreateMap<ProductCharacteristicValueUpdateModel, ProductCharacteristicDBModel>();

            CreateMap<ProductVideoDBModel, ProductVideoResponseModel>();
            CreateMap<ProductVideoCreateRequestModel, ProductVideoDBModel>();
            CreateMap<ProductVideoUpdateRequestModel, ProductVideoDBModel>()
                    .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            CreateMap<InstructionDBModel, InstructionResponseModel>();
            CreateMap<InstructionCreateRequestModel, InstructionDBModel>();
            CreateMap<InstructionUpdateRequestModel, InstructionDBModel>()
                    .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            CreateMap<FeedbackDBModel, FeedbackResponseModel>();
            CreateMap<FeedbackCreateRequestModel, FeedbackDBModel>();
            CreateMap<FeedbackUpdateRequestModel, FeedbackDBModel>()
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.User, opt => opt.Ignore())
                .ForMember(dest => dest.Product, opt => opt.Ignore())
                .ForMember(dest => dest.FeedbackImages, opt => opt.Ignore());

            CreateMap<ReviewDBModel, ReviewResponseModel>();
            CreateMap<ReviewCreateRequestModel, ReviewDBModel>();
            CreateMap<ReviewUpdateRequestModel, ReviewDBModel>();

            CreateMap<SellerDBModel, SellerResponseModel>();
            CreateMap<SellerCreateRequestModel, SellerDBModel>();
            CreateMap<SellerUpdateRequestModel, SellerDBModel>();
        }
    }
}
