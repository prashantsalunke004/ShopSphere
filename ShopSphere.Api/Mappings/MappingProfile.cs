using AutoMapper;
using ShopSphere.API.DTOs;
using ShopSphere.API.Models;

namespace ShopSphere.API.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile() 
        {
            CreateMap<Category, CategoryDto>();
            CreateMap<CreateCategoryDto, Category>();
            CreateMap<UpdateCategoryDto, Category>();
            CreateMap<Product, ProductDto>().ForMember(dest=>dest.CategoryName,opt=>opt.MapFrom(src=>src.Category.Name));
            CreateMap<CreateProductDto, Product>();
            //CreateMap<UpdateProductDto, Product>();
            CreateMap<UpdateProductDto, Product>().ForMember(dest => dest.ImageUrl,opt => opt.Ignore());
        }
    }
}
