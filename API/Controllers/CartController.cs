using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Application.UserModules.DTOs.CartItem;
using Application.UserModules.Abstracts;
using System.Threading.Tasks;
using System.Security.Claims;
using Shared.WebAPIBase;
using Application.UserModules.Implements;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ApiControllerBase
    {
        private readonly ICartService _cartService;
        private readonly ILogger<CartController> _logger;

        public CartController(ICartService cartService, ILogger<CartController> logger) : base(logger)
        {
            _cartService = cartService;
            _logger = logger;
        }

        [HttpPost("add")]
        public async Task<ApiResponse> AddItemToCart([FromBody] AddCartItemDto request)

        {
            try
            {
                var userId = GetCurrentUserId();
                if (string.IsNullOrEmpty(userId))
                {
                    _logger.LogWarning("User ID is null");
                    return new ApiResponse(0, null, 400, "User ID not found. Please ensure you are logged in.");
                }
                await _cartService.AddItemToCartAsync(request, int.Parse(userId));
                return new();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding order");
                return OkException(ex);
            }
        }

        [HttpPut("update-quantity")]
        public async Task<ApiResponse> UpdateCartItemQuantity(UpdateCartItemDto updateCartItem)
        {
            try
            {
                var userId = GetCurrentUserId();
                if (string.IsNullOrEmpty(userId))
                {
                    _logger.LogWarning("User ID is null");
                    return new ApiResponse(0, null, 400, "User ID not found. Please ensure you are logged in.");
                }
                await _cartService.UpdateCartItemQuantityAsync(int.Parse(userId), updateCartItem);
                return new();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating cart quantity");
                return OkException(ex);
            }
        }

        [HttpDelete("remove")]
        public async Task<ApiResponse> RemoveItemFromCart(int productId)
        {
            try
            {
                var userId = GetCurrentUserId();
                if (string.IsNullOrEmpty(userId))
                {
                    _logger.LogWarning("User ID is null");
                    return new ApiResponse(0, null, 400, "User ID not found. Please ensure you are logged in.");
                }
                await _cartService.RemoveItemFromCartAsync(int.Parse(userId), productId);
                return new();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating cart quantity");
                return OkException(ex);
            }
        }   

        [HttpGet("get-my-cart")]
        public async Task<ApiResponse> GetCart()
        {
            try
            {
                var userId = GetCurrentUserId();
                if (string.IsNullOrEmpty(userId))
                {
                    _logger.LogWarning("User ID is null");
                    return new ApiResponse(0, null, 400, "User ID not found. Please ensure you are logged in.");
                }

                _logger.LogInformation("Get cart by userId");
                return new(await _cartService.GetCartByUserIdAsync(int.Parse(userId)));

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching cart by id");
                return OkException(ex);
            }
        }
        private string GetCurrentUserId()
        {
            return User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        }
    }
}
