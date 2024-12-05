using Application.UserModules.DTOs.Product;
using AutoMapper;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UserModules.Profiles
{
    public class ProductProfile : Profile
    {
        public ProductProfile() 
        {
            CreateMap<AddProductDto, Product>();
            CreateMap<UpdateProductDto, Product>();
            CreateMap<Product, ProductDto>()
                .ForMember(dest => dest.ImageUrls, opt => opt.MapFrom(src => src.ProductImages.Select(pi => pi.ImageUrl)));
            //CreateMap<Product, ProductForSaleDto>();
        }
    }
}
