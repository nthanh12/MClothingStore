using Application.UserModules.DTOs.Order;
using Application.UserModules.DTOs.OrderDetail;
using AutoMapper;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UserModules.Profiles
{
    public class OrderProfile : Profile
    {
        public OrderProfile()
        {
            CreateMap<AddOrderWithDetailsDto, Order>()
                .ForMember(dest => dest.OrderDetails, opt => opt.Ignore());
            
            CreateMap<UpdateOrderWithDetailsDto, Order>()
                .ForMember(dest => dest.OrderDetails, opt => opt.Ignore());
            
            CreateMap<Order, OrderWithDetailsDto>()
                .ForMember(dest => dest.OrderDetails, opt => opt.MapFrom(src => src.OrderDetails))
                .ForMember(dest => dest.Customer, opt => opt.Ignore());
            
            CreateMap<Order, OrderDto>();
            
            CreateMap<AddOrderDetailDto, OrderDetail>()
                .ForMember(dest => dest.Product, opt => opt.Ignore());

            CreateMap<OrderDetail, OrderDetailDto>();
            CreateMap<OrderDetail, AddOrderDetailDto>();
        }
    }
}
