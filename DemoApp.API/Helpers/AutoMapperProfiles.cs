using System;
using AutoMapper;
using DemoApp.API.Data;
using DemoApp.API.Dtos;

namespace DemoApp.API.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            // CreateMap<Product, ProductDto>().ForMember(dest => dest.Price, options => options.MapFrom(src => src.Price.ToString("C0")))
            //                                 .ForMember(dest => dest.ProductCateName, options => options.MapFrom(src => src.ProductCategory.Description));
            CreateMap<CreateProductDto, Product>().ForMember(dest => dest.Price, options => options.MapFrom(src => Decimal.Parse(src.Price)))
                                            .ForMember(dest => dest.ProductCategory, options => options.Ignore());
            CreateMap<EditProductDto, Product>().ForMember(dest => dest.Price, options => options.MapFrom(src => Decimal.Parse(src.Price)))
                                            .ForMember(dest => dest.ProductCategory, options => options.Ignore())
                                            .ForMember(dest => dest.Id, options => options.MapFrom(src => src.ProductId));
            CreateMap<Product, ProductDto>().ForMember(dest => dest.ProductCateName, options => options.MapFrom(src => src.ProductCategory.Description));
        }
    }
}