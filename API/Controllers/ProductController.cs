using Application.UserModules.Abstracts;
using Application.UserModules.DTOs.Product;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shared.ApplicationBase.Common;
using Shared.WebAPIBase;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ApiControllerBase
    {
        private readonly IProductService _productService;
        private readonly ILogger<ProductController> _logger;

        public ProductController(IProductService productService, ILogger logger) : base(logger)
        {
            _productService = productService;
        }

        [HttpGet("find-all")]
        public async Task<ApiResponse> FindAll()
        {
            try
            {
                return new(await _productService.GetAllProductsAsync());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching all products");
                return OkException(ex);
            }
        }
        [HttpGet("paged")]
        public async Task<ApiResponse> GetPagedProductsAsync(ProductPagingRequestDto input)
        {
            try
            {
                return new(await _productService.GetPagedProductsAsync(input));
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error fetching paged products");
                return OkException(ex);
            }
        }

    }
}
