using Application.UserModules.Abstracts;
using Domain.Entities;
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
                // Kiểm tra danh mục trùng lặp (gọi phương thức Repository)
                var categoriesToAdd = await _productCategoryRepository.GetCategoriesToAddAsync(productId, categoryIds);

                if (categoriesToAdd.Any())
                {
                    await _productCategoryRepository.AssignProductToCategoriesAsync(productId, categoriesToAdd);
                    _logger.LogInformation($"Product {productId} assigned to new categories successfully.");
                }
                else
                {
                    _logger.LogInformation($"Product {productId} already has all the provided categories.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error assigning product {productId} to categories: {ex.Message}");
                throw;
            }
        }

        public async Task RemoveAllCategoriesFromProductAsync(int productId)
        {
            try
            {
                // Xóa tất cả danh mục của sản phẩm
                await _productCategoryRepository.RemoveAllCategoriesFromProductAsync(productId);
                _logger.LogInformation($"All categories removed from product {productId} successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error removing all categories from product {productId}: {ex.Message}");
                throw;
            }
        }

        // Xóa sản phẩm khỏi các danh mục cụ thể
        public async Task RemoveProductFromCategoriesAsync(int productId, List<int> categoryIds)
        {
            try
            {
                // Kiểm tra danh mục cần xóa
                var categoriesToRemove = await _productCategoryRepository.GetCategoriesByProductIdAsync(productId);
                var categoriesToRemoveIds = categoriesToRemove
                    .Where(c => categoryIds.Contains(c.CategoryId))
                    .Select(c => c.CategoryId)
                    .ToList();

                if (categoriesToRemoveIds.Any())
                {
                    await _productCategoryRepository.RemoveProductFromCategoriesAsync(productId, categoriesToRemoveIds);
                    _logger.LogInformation($"Product {productId} removed from categories successfully.");
                }
                else
                {
                    _logger.LogInformation($"Product {productId} does not belong to any of the provided categories.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error removing product {productId} from categories: {ex.Message}");
                throw;
            }
        }
    }
}
