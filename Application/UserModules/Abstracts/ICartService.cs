using Application.UserModules.DTOs.Cart;
using Application.UserModules.DTOs.CartItem;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UserModules.Abstracts
{
    public interface ICartService
    {
        Task<bool> AddItemToCartAsync(AddCartItemDto addCart, int userId);
        Task<Order> CreateOrderFromCartAsync(int userId);
        Task<bool> RemoveItemFromCartAsync(int userId, int productId);
        Task<bool> UpdateCartItemQuantityAsync(int userId, UpdateCartItemDto updateCartItemDto);
        Task<CartDto> GetCartByUserIdAsync(int userId);    
    }
}
