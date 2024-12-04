using Application.UserModules.Abstracts;
using Application.UserModules.DTOs.Category;
using Application.UserModules.Implements;
using Application.UserModules.Requests;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Shared.ApplicationBase.Common;
using Shared.WebAPIBase;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ApiControllerBase
    {
        private readonly ICategoryService _categoryService;
        private readonly ILogger<CategoryController> _logger;

        public CategoryController(ILogger<CategoryController> logger, ICategoryService categoryService) : base(logger)
        {
            _logger = logger;
            _categoryService = categoryService;
        }

        [HttpGet("find-all")]
        public async Task<ApiResponse> FindAll()
        {
            try
            {
                return new(await _categoryService.GetAllCategoriesAsync());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching all categories");
                return OkException(ex);
            }
        }

        [HttpGet("find-by-id")]
        public async Task<ApiResponse> FindById(int id)
        {
            try
            {
                return new(await _categoryService.GetCategoryByIdAsync(id));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching product by id");
                return OkException(ex);
            }
        }

        [HttpPost("add-category")]
        public async Task<ApiResponse> CreateCategoryAsync([FromBody] AddCategoryDto input)
        {
            try
            {
                await _categoryService.AddCategoryAsync(input);
                return new();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding category");
                return OkException(ex);
            }
        }

        [HttpPut("update-category")]
        public async Task<ApiResponse> UpdateCategoryAsync([FromBody] UpdateCategoryDto input)
        {
            try
            {
                await _categoryService.UpdateCategoryAsync(input);
                return new();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating category");
                return OkException(ex);
            }
        }

        [HttpDelete("delete/id")]
        public async Task<ApiResponse> DeleteCategoryAsync(int id)
        {
            try
            {
                await _categoryService.DeleteCategoryAsync(id);
                return new();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting category");
                return OkException(ex);
            }
        }
    }
}
