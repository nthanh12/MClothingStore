using Application.UserModules.DTOs.Category;
using Application.UserModules.DTOs.PaymentMethod;
using AutoMapper;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UserModules.Profiles
{
    public class PaymentMethodProfile : Profile
    {
        public PaymentMethodProfile()
        {
            CreateMap<AddPaymentMethodDto, PaymentMethod>();
            CreateMap<UpdatePaymentMethodDto, PaymentMethod>();
            CreateMap<PaymentMethod, PaymentMethodDto>();
        }
    }
}
