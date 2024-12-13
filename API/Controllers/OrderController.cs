using Application.UserModules.Abstracts;
using Application.UserModules.DTOs.Order;
using Application.UserModules.Implements;
using Application.UserModules.Requests;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shared.WebAPIBase;
using System.Security.Claims;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ApiControllerBase
    {
        private readonly ILogger<OrderController> _logger;
        private readonly IOrderService _orderService;
        private readonly IUserCustomerService _userCustomerService;
        public OrderController(IOrderService orderService, IUserCustomerService userCustomerService, ILogger<OrderController> logger) : base(logger)
        {
            _logger = logger;
            _userCustomerService = userCustomerService;
            _orderService = orderService;
        }

        [HttpGet("find-all")]
        public async Task<ApiResponse> FindAll()
        {
            try
            {
                return new(await _orderService.GetAllOrdersAsync());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching all order");
                return OkException(ex);
            }
        }

        [HttpGet("find-by-id")]
        public async Task<ApiResponse> FindById(int id)
        {
            try
            {
                return new(await _orderService.GetOrderByIdAsync(id));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching order by id");
                return OkException(ex);
            }
        }

        [HttpPost("add-order")]
        public async Task<ApiResponse> CreateOrderAsync([FromBody] AddOrderWithDetailsDto orderDto)
        {
            try
            {
                var userId = GetCurrentUserId();
                if (string.IsNullOrEmpty(userId))
                {
                    _logger.LogWarning("User ID is null");
                    return new ApiResponse(0, null, 400, "User ID not found. Please ensure you are logged in.");
                }

                var custId = await _userCustomerService.GetCustomerIdByUserIdAsync(int.Parse(userId));
                orderDto.CustomerId = custId;
                await _orderService.AddOrderAsync(orderDto);
                return new();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding order");
                return OkException(ex);
            }
        }

        [HttpPost("checkout")]
        public async Task<ApiResponse> CreateOrderFromCart()
        {
            try
            {
                var user = GetCurrentUserId();
                if (string.IsNullOrEmpty(user))
                {
                    _logger.LogWarning("User ID is null");
                    return new ApiResponse(0, null, 400, "User ID not found. Please ensure you are logged in.");
                }
                var userId = int.Parse(user);
                var custId = await _userCustomerService.GetCustomerIdByUserIdAsync(userId);

                await _orderService.CreateOrderFromCartAsync(userId, custId);
                return new();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding order");
                return OkException(ex);
            }
        }

        [HttpGet("get-my-order")]
        public async Task<ApiResponse> GetAllMyOrder()
        {
            try
            {
                var userId = GetCurrentUserId();
                if (string.IsNullOrEmpty(userId))
                {
                    _logger.LogWarning("User ID is null");
                    return new ApiResponse(0, null, 400, "User ID not found. Please ensure you are logged in.");
                }

                var custId = await _userCustomerService.GetCustomerIdByUserIdAsync(int.Parse(userId));
                _logger.LogInformation("Get customerId by userId");
                return new(await _orderService.GetOrderByCustomerIdAsync(custId));

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching order by id");
                return OkException(ex);
            }
        }

        [HttpPut("update-order")]
        public async Task<ApiResponse> UpdateOrderAsync([FromBody] UpdateOrderWithDetailsDto orderDto)
        {
            try
            {
                var userId = GetCurrentUserId();
                if (string.IsNullOrEmpty(userId))
                {
                    _logger.LogWarning("User ID is null");
                    return new ApiResponse(0, null, 400, "User ID not found. Please ensure you are logged in.");
                }

                var custId = await _userCustomerService.GetCustomerIdByUserIdAsync(int.Parse(userId));
                orderDto.CustomerId = custId;
                await _orderService.UpdateOrderAsync(orderDto);
                return new();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating order");
                return OkException(ex);
            }
        }

        [HttpDelete("delete/{id}")]
        public async Task<ApiResponse> DeleteOrderAsync(int id)
        {
            try
            {
                await _orderService.DeleteOrderAsync(id);
                return new();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting order");
                return OkException(ex);
            }
        }

        private string GetCurrentUserId()
        {
            return User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        }
    }
}
