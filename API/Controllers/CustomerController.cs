using Application.UserModules.Abstracts;
using Application.UserModules.DTOs.Customer;
using Application.UserModules.Implements;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shared.WebAPIBase;
using System.Security.Claims;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ApiControllerBase
    {
        private readonly ICustomerService _customerService;
        private readonly ILogger<CustomerController> _logger;

        public CustomerController(ICustomerService customerService, ILogger<CustomerController> logger) : base(logger)
        {
            _customerService = customerService;
            _logger = logger;
        }

        [HttpPost("register")]
        public async Task<ApiResponse> Register([FromBody] RegisterCustomerDto customerDto)
        {
            try
            {
                var userId = GetCurrentUserId();
                if (string.IsNullOrEmpty(userId))
                {
                    _logger.LogWarning("User ID is null");
                    return new ApiResponse(0, null, 400, "User ID not found. Please ensure you are logged in.");
                }
                await _customerService.RegisterCustomerAsync(customerDto, int.Parse(userId));
                return new();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding category");
                return OkException(ex);
            }
        }
        private string GetCurrentUserId() 
        { 
            return User.FindFirst(ClaimTypes.NameIdentifier)?.Value; 
        }
    }
}
