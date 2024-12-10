using Application.UserModules.DTOs.Payment;
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
    public class PaymentProfile : Profile
    {
        public PaymentProfile() 
        {
            CreateMap<AddPaymentDto, Payment>();

        }
    }
}
