using Application.UserModules.Abstracts;
using Domain.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Application.UserModules.Implements
{
    public class ProductCategoryService : IProductCategoryService
    {
        private readonly IProductCategoryRepository _productCategoryRepository;
        private readonly ILogger<ProductCategoryService> _logger;

        public ProductCategoryService(
            IProductCategoryRepository productCategoryRepository,
            ILogger<ProductCategoryService> logger)
        {
            _productCategoryRepository = productCategoryRepository;
            _logger = logger;
        }

        // Gán sản phẩm vào các danh mục
        public async Task AssignProductToCategoriesAsync(int productId, List<int> categoryIds)
        {
            try
            {
                await _productCategoryRepository.AssignCategoriesAsync(productId, categoryIds);
                _logger.LogInformation($"Product {productId} assigned to categories successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error assigning product {productId} to categories: {ex.Message}");
                throw;
            }
        }

        // Gỡ sản phẩm khỏi danh mục cụ thể
        public async Task RemoveProductFromCategoriesAsync(int productId, List<int> categoryIds)
        {
            try
            {
                foreach (var categoryId in categoryIds)
                {
                    await _productCategoryRepository.RemoveProductFromCategoryAsync(productId, categoryId);
                    _logger.LogInformation($"Product {productId} removed from category {categoryId}.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error removing product {productId} from categories: {ex.Message}");
                throw;
            }
        }

        // Gỡ sản phẩm khỏi tất cả các danh mục mà nó thuộc về
        public async Task RemoveProductFromAllCategoriesAsync(int productId)
        {
            try
            {
                // Lấy danh sách các danh mục mà sản phẩm này thuộc về
                var categories = await _productCategoryRepository.GetCategoriesByProductIdAsync(productId);

                if (categories.Any())
                {
                    // Xóa liên kết giữa sản phẩm và tất cả các danh mục
                    foreach (var category in categories)
                    {
                        await _productCategoryRepository.RemoveProductFromCategoryAsync(productId, category.Id);
                        _logger.LogInformation($"Product {productId} removed from category {category.Id}.");
                    }

                    _logger.LogInformation($"Product {productId} removed from all categories.");
                }
                else
                {
                    _logger.LogWarning($"No categories found for product {productId}.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error removing product {productId} from all categories: {ex.Message}");
                throw;
            }
        }
    }
}
