using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UserModules.DTOs.Payment
{
    public class AddPaymentDto
    {
        public int OrderID { get; set; }
        public int PaymentMethodID { get; set; }
        public DateTime PaymentDate { get; set; }
        public decimal Amount { get; set; }
    }
}
