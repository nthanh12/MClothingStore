using Application.UserModules.DTOs.Order;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UserModules.Abstracts
{
    public interface IOrderService
    {
        Task<IEnumerable<OrderDto>> GetAllOrdersAsync();
        Task<OrderWithDetailsDto> GetOrderByIdAsync(int id);
        Task AddOrderAsync(AddOrderWithDetailsDto orderDto);
        Task UpdateOrderAsync(UpdateOrderWithDetailsDto orderDto);
        Task DeleteOrderAsync(int id);
        Task<IEnumerable<OrderWithDetailsDto>> GetOrderByCustomerIdAsync(int customerId);
    }
}
