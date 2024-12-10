using Application.UserModules.Abstracts;
using Application.UserModules.DTOs.Payment;
using Application.UserModules.Implements;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shared.WebAPIBase;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ApiControllerBase
    {
        private readonly ILogger<PaymentController> _logger;
        private readonly IPaymentService _paymentService;

        public PaymentController(ILogger<PaymentController> logger, IPaymentService paymentService) : base(logger)
        {
            _logger = logger;
            _paymentService = paymentService;
        }

        [HttpPost("create")]
        public async Task<ApiResponse> CreatePayment([FromBody] AddPaymentDto paymentDto)
        {
            try
            {
                await _paymentService.AddPaymentAsync(paymentDto);
                return new();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding payment");
                return OkException(ex);
            }
        }
    }
}
