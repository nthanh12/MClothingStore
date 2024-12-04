using Application.UserModules.Abstracts;
using Application.UserModules.DTOs.Product;
using Application.UserModules.Requests;
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

        public ProductController(IProductService productService, ILogger<ProductController> logger) : base(logger)
        {
            _productService = productService;
            _logger = logger;
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

        [HttpGet("find-by-id")]
        public async Task<ApiResponse> FindById(int id)
        {
            try
            {
                return new(await _productService.GetProductByIdAsync(id));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching product by id");
                return OkException(ex);
            }
        }

        [HttpPost("add-product")]
        public async Task<ApiResponse> CreateProductAsync([FromBody] AddProductRequest input)
        {
            try
            {
                await _productService.AddProductAsync(input.ProductDto, input.CategoryIds, input.ProductImagesDto);
                return new();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding product");
                return OkException(ex);
            }
        }

        [HttpPut("update-product")]
        public async Task<ApiResponse> UpdateProductAsync([FromBody] UpdateProductRequest input)
        {
            try
            {
                await _productService.UpdateProductAsync(input.ProductDto, input.CategoryIds, input.ProductImagesDto);
                return new();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding product");
                return OkException(ex);
            }
        }

        [HttpDelete("delete/id")]
        public async Task<ApiResponse> DeleteProductAsync(int id)
        {
            try
            {
                await _productService.DeleteProductAsync(id);
                return new();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting product");
                return OkException(ex);
            }
        }
    }
}
