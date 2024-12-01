using Application.UserModules.Abstracts;
using Application.UserModules.DTOs.Product;
using Application.UserModules.DTOs.ProductImage;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using Microsoft.Extensions.Logging;
using Shared.ApplicationBase.Common;
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
        private readonly IMapper _mapper;

        public ProductService(
            IProductRepository productRepository,
            IProductCategoryService productCategoryService,
            IProductImageRepository productImageRepository,
            ILogger<ProductService> logger,
            IMapper mapper)
        {
            _productRepository = productRepository;
            _productCategoryService = productCategoryService;
            _productImageRepository = productImageRepository;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task AddProductAsync(AddProductDto productDto, List<int> categoryIds, List<ProductImageDto> productImagesDto)
        {
            try
            {
                var product = _mapper.Map<Product>(productDto);
                var productImages = _mapper.Map<List<ProductImage>>(productImagesDto); 

                await _productRepository.AddAsync(product);
                await _productCategoryService.AssignProductToCategoriesAsync(product.Id, categoryIds);
                await _productImageRepository.AddImageAsync(product.Id, productImages);
                _logger.LogInformation($"Product {product.Name} added successfully with categories and images.");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error adding product {productDto.Name}: {ex.Message}");
                throw;
            }
        }

        public async Task DeleteProductAsync(int productId)
        {
            try
            {
                // Kiểm tra xem sản phẩm có tồn tại không
                var product = await _productRepository.GetByIdAsync(productId);
                if (product == null)
                {
                    throw new Exception("Product not found");
                }

                // Gỡ sản phẩm khỏi tất cả các danh mục
                await _productCategoryService.RemoveProductFromAllCategoriesAsync(productId);

                // Xóa tất cả các ảnh liên quan đến sản phẩm
                await _productImageRepository.DeleteImagesByProductIdAsync(productId);

                // Xóa sản phẩm khỏi database
                await _productRepository.DeleteAsync(productId);

                _logger.LogInformation($"Product {productId} and its images deleted successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error deleting product {productId}: {ex.Message}");
                throw;
            }
        }

        public async Task<PagingResult<ProductDto>> GetAllProductsAsync()
        {
            var result = await _productRepository.GetAllAsync();
            var productDtos = _mapper.Map<IEnumerable<ProductDto>>(result);
            return new PagingResult<ProductDto>
            {
                Items = productDtos,
                TotalItems = result.Count()
            };
        }

        public Task<IEnumerable<ProductDto>> GetPagedProductsAsync(PagingRequestBaseDto input)
        {
            throw new NotImplementedException();
        }

        public async Task<ProductDto> GetProductByIdAsync(int productId)
        {
            var product = await _productRepository.GetByIdAsync(productId);
            if (product == null)
            {
                return null;
            }

            var productDto = _mapper.Map<ProductDto>(product);
            return productDto;
        }

        public async Task UpdateProductAsync(UpdateProductDto productDto, List<int> newCategoryIds, List<ProductImageDto> newProductImagesDto)
        {
            try
            {
                var product = _mapper.Map<Product>(productDto);
                var productImages = _mapper.Map<List<ProductImage>>(newProductImagesDto);

                await _productRepository.UpdateAsync(product);
                await _productCategoryService.AssignProductToCategoriesAsync(product.Id, newCategoryIds);
                await _productImageRepository.UpdateImageAsync(product.Id, productImages);
                _logger.LogInformation($"Product {product.Name} updated successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error updating product {productDto.Name}: {ex.Message}");
                throw;
            }
        }
    }
}
