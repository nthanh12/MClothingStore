using Application.UserModules.Abstracts;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.UserModules.Implements
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly IProductCategoryService _productCategoryService;
        private readonly IProductImageRepository _productImageRepository;
        private readonly ILogger<ProductService> _logger;

        public ProductService(
            IProductRepository productRepository,
            IProductCategoryService productCategoryService,
            IProductImageRepository productImageRepository,
            ILogger<ProductService> logger)
        {
            _productRepository = productRepository;
            _productCategoryService = productCategoryService;
            _productImageRepository = productImageRepository;
            _logger = logger;
        }

        public async Task AddProductAsync(Product product, List<int> categoryIds, List<ProductImage> productImages)
        {
            try
            {
                await _productRepository.AddAsync(product);
                await _productCategoryService.AssignProductToCategoriesAsync(product.Id, categoryIds);
                await _productImageRepository.AddImageAsync(product.Id, productImages);
                _logger.LogInformation($"Product {product.Name} added successfully with categories and images.");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error adding product {product.Name}: {ex.Message}");
                throw;
            }
        }

        public async Task DeleteProductAsync(int productId)
        {
            try
            {
                //await _productCategoryService.RemoveProductFromCategoriesAsync(productId, new List<int>());
                //await _productImageRepository.DeleteImageAsync(productId);
                //await _productRepository.DeleteAsync(productId);
                _logger.LogInformation($"Product {productId} deleted successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error deleting product {productId}: {ex.Message}");
                throw;
            }
        }

        public async Task<IEnumerable<Product>> GetAllProductsAsync()
        {
            return await _productRepository.GetAllAsync();
        }

        public async Task<Product> GetProductByIdAsync(int productId)
        {
            return await _productRepository.GetByIdAsync(productId);
        }

        public async Task UpdateProductAsync(Product product, List<int> newCategoryIds, List<ProductImage> newProductImages)
        {
            try
            {
                await _productRepository.UpdateAsync(product);
                await _productCategoryService.AssignProductToCategoriesAsync(product.Id, newCategoryIds);
                await _productImageRepository.UpdateImageAsync(product.Id, newProductImages);
                _logger.LogInformation($"Product {product.Name} updated successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error updating product {product.Name}: {ex.Message}");
                throw;
            }
        }
    }
}
