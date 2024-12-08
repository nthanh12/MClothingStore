using Application.UserModules.Abstracts;
using Application.UserModules.DTOs.PaymentMethod;
using Application.UserModules.Implements;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shared.WebAPIBase;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentMethodController : ApiControllerBase
    {
        private readonly IPaymentMethodService _paymentMethodService;
        private readonly ILogger<PaymentMethodController> _logger;
        public PaymentMethodController(ILogger<PaymentMethodController> logger, IPaymentMethodService paymentMethodService) : base(logger)
        {
            _paymentMethodService = paymentMethodService;
            _logger = logger;
        }

        [HttpGet("find-all")]
        public async Task<ApiResponse> FindAll()
        {
            try
            {
                return new(await _paymentMethodService.GetAllAsync());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching all payment methods");
                return OkException(ex);
            }
        }

        [HttpGet("find-by-id")]
        public async Task<ApiResponse> FindById(int id)
        {
            try
            {
                return new(await _paymentMethodService.GetByIdAsync(id));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching payment method by id");
                return OkException(ex);
            }
        }

        [HttpPost("add-payment-method")]
        public async Task<ApiResponse> CreatePaymentMethodAsync([FromBody] AddPaymentMethodDto input)
        {
            try
            {
                await _paymentMethodService.AddAsync(input);
                return new();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding payment method");
                return OkException(ex);
            }
        }

        [HttpPut("update-payment-method")]
        public async Task<ApiResponse> UpdatePaymentMethodAsync([FromBody] UpdatePaymentMethodDto input)
        {
            try
            {
                await _paymentMethodService.UpdateAsync(input);
                return new();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating payment method");
                return OkException(ex);
            }
        }

        [HttpDelete("delete/{id}")]
        public async Task<ApiResponse> DeletePaymentMethodAsync(int id)
        {
            try
            {
                await _paymentMethodService.DeleteAsync(id);
                return new();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting payment method");
                return OkException(ex);
            }
        }
    }
}
