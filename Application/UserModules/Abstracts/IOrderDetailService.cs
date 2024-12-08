using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UserModules.Abstracts
{
    public interface IOrderDetailService
    {
        Task<IEnumerable<OrderDetail>> GetAllByOrderIdAsync(int orderId); 
        Task<OrderDetail?> GetByIdAsync(int id); 
        Task AddAsync(OrderDetail orderDetail); 
        Task UpdateAsync(OrderDetail orderDetail); 
        Task DeleteAsync(int id);
    }
}
