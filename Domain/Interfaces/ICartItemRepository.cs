using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface ICartItemRepository
    {
        Task<CartItem> GetCartItemByIdAsync(int cartItemId); 
        Task AddAsync(CartItem cartItem); 
        Task UpdateAsync(CartItem cartItem); 
        Task DeleteAsync(int cartItemId);
    }
}
