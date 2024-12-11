using Application.UserModules.DTOs.CartItem;
using Application.UserModules.DTOs.Category;
using AutoMapper;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UserModules.Profiles
{
    public class CartItemProfile : Profile
    {
        public CartItemProfile() 
        {
            CreateMap<AddCartItemDto, CartItem>();
            CreateMap<UpdateCartItemDto, CartItem>();
            CreateMap<CartItem, CartItemDto>();
        }
    }
}
