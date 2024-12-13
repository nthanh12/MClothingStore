using Application.UserModules.Abstracts;
using Application.UserModules.DTOs.CartItem;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace Application.UserModules.Implements
{
    public class CartItemService : ICartItemService
    {
        private readonly ICartItemRepository _cartItemRepository;
        private readonly IProductRepository _productRepository;
        private readonly ILogger<CartItemService> _logger;
        private readonly IMapper _mapper;

        public CartItemService(ICartItemRepository cartItemRepository, IProductRepository productRepository, ILogger<CartItemService> logger, IMapper mapper)
        {
            _cartItemRepository = cartItemRepository;
            _productRepository = productRepository;
            _logger = logger;
            _mapper = mapper;
        }
        public async Task<bool> AddCartItemAsync(AddCartItemDto addCart, int cartId)
        {
            try
            {
                var product = await _productRepository.GetByIdAsync(addCart.ProductId);
                if (product == null)
                {
                    _logger.LogWarning($"Product with ID {addCart.ProductId} not found.");
                    return false;
                }

                var cartItem = _mapper.Map<CartItem>(addCart);
                cartItem.CartId = cartId;
                cartItem.CreatedDate = DateTime.UtcNow;

                await _cartItemRepository.AddAsync(cartItem);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding item to cart");
                return false;
            }
        }
        public async Task<bool> UpdateCartItemAsync(UpdateCartItemDto cartItemDto)
        {
            try
            {
                var existingCartItem = await _cartItemRepository.GetCartItemByIdAsync(cartItemDto.Id);
                if (existingCartItem == null)
                {
                    _logger.LogWarning($"Cart item with ID {cartItemDto.Id} not found.");
                    return false;
                }

                _mapper.Map(cartItemDto, existingCartItem); // Ánh xạ DTO sang thực thể CartItem
                await _cartItemRepository.UpdateAsync(existingCartItem);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating cart item");
                return false;
            }
        }
        public async Task<bool> RemoveCartItemAsync(int cartItemId)
        {
            try
            {
                await _cartItemRepository.DeleteAsync(cartItemId);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error removing item from cart");
                return false;
            }
        }
    }
}
