using AutoMapper;
using Domain.Models.DBModels;
using Domain.Models.Request.Categories;
using Domain.Models.Request.Products;
using Domain.Models.Response.Categories;
using Domain.Models.Response.Products;

namespace PriceComparisonWebAPI.Infrastructure
{
    public class AppMappingProfile : Profile
    {
        public AppMappingProfile()
        {
            CreateMap<CategoryDBModel, CategoryResponseModel>();
            CreateMap<CategoryDBModel, CategoryRequestModel>();
            CreateMap<CategoryRequestModel, CategoryDBModel>();
            CreateMap<CategoryCreateRequestModel, CategoryDBModel>();
            CreateMap<CategoryUpdateRequestModel, CategoryDBModel>();

            CreateMap<CharacteristicDBModel, CharacteristicResponseModel>();
            CreateMap<CharacteristicRequestModel, CharacteristicDBModel>();

            CreateMap<RelatedCategoryDBModel, RelatedCategoryResponseModel>();
            CreateMap<RelatedCategoryRequestModel, RelatedCategoryDBModel>();

            CreateMap<CategoryCharacteristicDBModel, CategoryCharacteristicResponseModel>()
                .ForMember(dest => dest.CharacteristicTitle, opt => opt.MapFrom(src => src.Characteristic.Title))
                .ForMember(dest => dest.CharacteristicDataType, opt => opt.MapFrom(src => src.Characteristic.DataType))
                .ForMember(dest => dest.CharacteristicUnit, opt => opt.MapFrom(src => src.Characteristic.Unit));
            CreateMap<CategoryCharacteristicRequestModel, CategoryCharacteristicDBModel>();




        }
    }
}
