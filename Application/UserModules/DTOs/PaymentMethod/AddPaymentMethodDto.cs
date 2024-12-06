using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UserModules.DTOs.PaymentMethod
{
    public class AddPaymentMethodDto
    {
        public string Name { get; set; } = null!;
    }
}
