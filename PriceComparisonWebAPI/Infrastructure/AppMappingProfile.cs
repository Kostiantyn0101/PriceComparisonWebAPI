using AutoMapper;
using Domain.Models.DBModels;
using PriceComparisonWebAPI.ViewModels;
using PriceComparisonWebAPI.ViewModels.Category;

namespace PriceComparisonWebAPI.Infrastructure
{
    public class AppMappingProfile : Profile
    {
        public AppMappingProfile()
        {
            CreateMap<CategoryDBModel, CategoryResponseViewModel>();
            CreateMap<CategoryRequestViewModel, CategoryDBModel>();

            CreateMap<CharacteristicDBModel, CharacteristicResponseViewModel>();
            CreateMap<CharacteristicRequestViewModel, CharacteristicDBModel>();

            CreateMap<RelatedCategoryDBModel, RelatedCategoryResponseViewModel>();
            CreateMap<RelatedCategoryRequestViewModel, RelatedCategoryDBModel>();

            CreateMap<CategoryCharacteristicDBModel, CategoryCharacteristicResponseViewModel>()
                .ForMember(dest => dest.CharacteristicTitle, opt => opt.MapFrom(src => src.Characteristic.Title))
                .ForMember(dest => dest.CharacteristicDataType, opt => opt.MapFrom(src => src.Characteristic.DataType))
                .ForMember(dest => dest.CharacteristicUnit, opt => opt.MapFrom(src => src.Characteristic.Unit));
            CreateMap<CategoryCharacteristicRequestViewModel, CategoryCharacteristicDBModel>();



        }
    }
}
