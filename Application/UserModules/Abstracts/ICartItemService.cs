using Application.UserModules.DTOs.CartItem;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UserModules.Abstracts
{
    public interface ICartItemService
    {
        Task<bool> AddCartItemAsync(AddCartItemDto addCart, int cartId);
        Task<bool> UpdateCartItemAsync(UpdateCartItemDto cartItem);
        Task<bool> RemoveCartItemAsync(int cartItemId);
    }

}
