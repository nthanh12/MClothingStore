using Application.UserModules.DTOs.Product;
using Application.UserModules.DTOs.ProductImage;
using AutoMapper;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UserModules.Profiles
{
    public class ProductImageProfile : Profile
    {
        public ProductImageProfile() 
        {
            CreateMap<AddProductImageDto, ProductImage>();
            CreateMap<UpdateProductImageDto, ProductImage>();
            CreateMap<Product, ProductImageDto>();
        }
    }
}
