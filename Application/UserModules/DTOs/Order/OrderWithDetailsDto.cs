using Application.UserModules.DTOs.Customer;
using Application.UserModules.DTOs.OrderDetail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UserModules.DTOs.Order
{
    public class OrderWithDetailsDto
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }
        public List<OrderDetailDto> OrderDetails { get; set; }
        public CustomerDto Customer { get; set; }
    }
}
