using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UserModules.DTOs.PaymentMethod
{
    public class UpdatePaymentMethodDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public ICollection<Payment>? Payments { get; set; }
    }
}
