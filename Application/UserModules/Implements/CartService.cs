using Application.UserModules.Abstracts;
using Application.UserModules.DTOs.Cart;
using Application.UserModules.DTOs.CartItem;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Application.UserModules.Implements
{
    public class CartService : ICartService
    {
        private readonly ICartRepository _cartRepository;
        private readonly ICartItemService _cartItemService;
        private readonly ILogger<CartService> _logger;
        private readonly IMapper _mapper;

        // private readonly IMemoryCache _cache;
        // private const string CartCacheKey = "UserCart_";

        public CartService(ICartRepository cartRepository, ICartItemService cartItemService, ILogger<CartService> logger, IMapper mapper /*, IMemoryCache cache */)
        {
            _cartRepository = cartRepository;
            _cartItemService = cartItemService;
            _logger = logger;
            _mapper = mapper;

            // _cache = cache;
        }

        public async Task<bool> AddItemToCartAsync(AddCartItemDto addCart, int userId)
        {
            try
            {
                var cart = await GetOrCreateCartAsync(userId);
                var existCartItem = cart.Items.FirstOrDefault(item => item.ProductId == addCart.ProductId);
                if (existCartItem != null)
                {
                    if (existCartItem.Quantity != addCart.Quantity)
                    {
                        var updateCartItemDto = new UpdateCartItemDto
                        {
                            Id = existCartItem.Id,
                            ProductId = existCartItem.ProductId,
                            Quantity = addCart.Quantity 
                        }; 
                        return await _cartItemService.UpdateCartItemAsync(updateCartItemDto); 
                    }
                    return true;
                    }
                else
                {
                    return await _cartItemService.AddCartItemAsync(addCart, cart.Id);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding item to cart");
                return false;
            }
        }
        public async Task<CartDto> GetCartByUserIdAsync(int userId)
        {
            try
            {
                var cart = await _cartRepository.GetCartByUserIdAsync(userId);
                if (cart == null)
                {
                    _logger.LogWarning($"No cart found for user ID {userId}");
                    return null;
                }
                return _mapper.Map<CartDto>(cart);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error retrieving cart for user ID {userId}");
                throw;
            }
        }

        public async Task<bool> RemoveItemFromCartAsync(int userId, int productId)
        {
            // Implementation here if needed
            throw new NotImplementedException();
        }

        public async Task<bool> UpdateCartItemQuantityAsync(int userId, UpdateCartItemDto updateCartItemDto)
        {
            try
            {
                var cart = await _cartRepository.GetCartByUserIdAsync(userId);
                if (cart == null)
                {
                    _logger.LogWarning($"No cart found for user ID {userId}");
                    return false;
                }
                var cartItem = cart.Items.FirstOrDefault(item => item.ProductId == updateCartItemDto.ProductId);
                if (cartItem == null)
                {
                    _logger.LogWarning($"No cart item found for product ID {updateCartItemDto.ProductId} in cart for user ID {userId}");
                    return false;
                }

                _mapper.Map(updateCartItemDto, cartItem);
                await _cartItemService.UpdateCartItemAsync(updateCartItemDto);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error updating cart item quantity for user ID {userId} and product ID {updateCartItemDto.ProductId}");
                return false;
            }
        }

        private async Task<Cart> GetOrCreateCartAsync(int userId)
        {
            // Commented out MemoryCache retrieval for future use
            // if (!_cache.TryGetValue(CartCacheKey + userId, out Cart cart))
            // {
            var cart = await _cartRepository.GetCartByUserIdAsync(userId);
            if (cart == null)
            {
                var addCart = new AddCartDto
                {
                    UserId = userId,
                    CreatedDate = DateTime.UtcNow
                };
                cart = _mapper.Map<Cart>(addCart);
                await _cartRepository.AddAsync(cart);
            }
            // Commented out MemoryCache storage for future use
            // _cache.Set(CartCacheKey + userId, cart, TimeSpan.FromMinutes(30)); // Store in cache
            // }
            return cart;
        }

        public Task<Order> CreateOrderFromCartAsync(int userId)
        {
            // Implementation here if needed
            throw new NotImplementedException();
        }

        #region Increase Quantity
        //public async Task<bool> IncreaseQuantityAsync(int userId, int productId)
        //{
        //    try
        //    {
        //        var cart = await GetOrCreateCartAsync(userId);
        //        var existCartItem = cart.Items.FirstOrDefault(item => item.ProductId == productId);
        //        if (existCartItem == null)
        //        {
        //            _logger.LogWarning($"No cart item found for product ID {productId} in cart for user ID {userId}");
        //            return false;
        //        }

        //        existCartItem.Quantity++;
        //        _cache.Set(CartCacheKey + userId, cart); // Update cache

        //        return await _cartItemService.UpdateCartItemAsync(new UpdateCartItemDto
        //        {
        //            Id = existCartItem.Id,
        //            ProductId = existCartItem.ProductId,
        //            Quantity = existCartItem.Quantity
        //        });
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, $"Error increasing quantity for user ID {userId} and product ID {productId}");
        //        return false;
        //    }
        //}
        
        //public async Task<bool> DecreaseQuantityAsync(int userId, int productId)
        //{
        //    try
        //    {
        //        var cart = await GetOrCreateCartAsync(userId);
        //        var existCartItem = cart.Items.FirstOrDefault(item => item.ProductId == productId);
        //        if (existCartItem == null)
        //        {
        //            _logger.LogWarning($"No cart item found for product ID {productId} in cart for user ID {userId}");
        //            return false;
        //        }

        //        if (existCartItem.Quantity > 1)
        //        {
        //            existCartItem.Quantity--;
        //            _cache.Set(CartCacheKey + userId, cart); // Update cache

        //            return await _cartItemService.UpdateCartItemAsync(new UpdateCartItemDto
        //            {
        //                Id = existCartItem.Id,
        //                ProductId = existCartItem.ProductId,
        //                Quantity = existCartItem.Quantity
        //            });
        //        }

        //        return false;
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError(ex, $"Error decreasing quantity for user ID {userId} and product ID {productId}");
        //        return false;
        //    }
        //}
        #endregion
        
    }
}
