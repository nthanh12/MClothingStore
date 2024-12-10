using Application.UserModules.DTOs.Payment;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UserModules.Abstracts
{
    public interface IPaymentService
    {
        Task<bool> AddPaymentAsync(AddPaymentDto paymentDto);
    }
}
