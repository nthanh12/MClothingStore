using Application.UserModules.Abstracts;
using Application.UserModules.DTOs.Customer;
using Application.UserModules.DTOs.Order;
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
        private readonly IUserCustomerService _userCustomerService;

        public CustomerController(ICustomerService customerService, ILogger<CustomerController> logger, IUserCustomerService userCustomerService) : base(logger)
        {
            _customerService = customerService;
            _logger = logger;
            _userCustomerService = userCustomerService;
        }

        [HttpPost("register")]
        public async Task<ApiResponse> Register([FromBody] UpdateCustomerDto customerDto)
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
                _logger.LogError(ex, "Error register custormer information");
                return OkException(ex);
            }
        }

        [HttpPut("update")]
        public async Task<ApiResponse> UpdateProfile([FromBody] UpdateCustomerDto customerDto)
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
                customerDto.Id = custId;

                await _customerService.UpdateCustomerAsync(customerDto);
                return new();

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error update profile customer information");
                return OkException(ex);
            }
        }
        private string GetCurrentUserId() 
        { 
            return User.FindFirst(ClaimTypes.NameIdentifier)?.Value; 
        }
    }
}
