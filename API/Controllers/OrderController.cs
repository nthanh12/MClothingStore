using Application.UserModules.Abstracts;
using Application.UserModules.DTOs.Order;
using Application.UserModules.Implements;
using Application.UserModules.Requests;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shared.WebAPIBase;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ApiControllerBase
    {
        private readonly ILogger<OrderController> _logger;
        private readonly IOrderService _orderService;
        public OrderController(IOrderService orderService, ILogger<OrderController> logger) : base(logger)
        {
            _logger = logger;
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
                await _orderService.AddOrderAsync(orderDto);
                return new();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding order");
                return OkException(ex);
            }
        }

        [HttpPut("update-order")]
        public async Task<ApiResponse> UpdateOrderAsync([FromBody] UpdateOrderWithDetailsDto orderDto)
        {
            try
            {
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
    }
}
