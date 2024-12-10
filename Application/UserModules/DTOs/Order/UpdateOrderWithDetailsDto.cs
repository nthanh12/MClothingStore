using Application.UserModules.DTOs.OrderDetail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UserModules.DTOs.Order
{
    public class UpdateOrderWithDetailsDto
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal DiscountRate { get; set; }
        public decimal TotalAmount { get; set; }
        public List<AddOrderDetailDto> OrderDetails { get; set; }
    }
}
