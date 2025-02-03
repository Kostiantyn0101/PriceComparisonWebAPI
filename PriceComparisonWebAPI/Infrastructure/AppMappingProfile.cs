using AutoMapper;
using Domain.Models.DBModels;
using Domain.Models.DTO.Categories;
using PriceComparisonWebAPI.ViewModels.Category;

namespace PriceComparisonWebAPI.Infrastructure
{
    public class AppMappingProfile : Profile
    {
        public AppMappingProfile()
        {
            CreateMap<CategoryDBModel, CategoryRequestViewModel>();
            CreateMap<CategoryRequestViewModel, CategoryDBModel>();
            CreateMap<CategoryCreateDto, CategoryDBModel>();
        }
    }
}
