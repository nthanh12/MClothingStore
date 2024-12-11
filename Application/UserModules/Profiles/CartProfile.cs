using Application.UserModules.DTOs.Cart;
using AutoMapper;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UserModules.Profiles
{
    public class CartProfile : Profile
    {
        public CartProfile() 
        {
            CreateMap<AddCartDto, Cart>()
                .ForMember(dest => dest.Items, opt => opt
            .MapFrom(src => new List<CartItem>()));
            CreateMap<Cart, CartDto>();
        }
    }
}
