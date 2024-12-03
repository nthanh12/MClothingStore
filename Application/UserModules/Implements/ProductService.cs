using Application.UserModules.Abstracts;
using Application.UserModules.DTOs.Product;
using Application.UserModules.DTOs.ProductImage;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Persistances;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Shared.ApplicationBase.Common;
using System;
using System.Collections.Generic;
using System.Text.Json;
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
        public readonly ApplicationDbContext _context;

        public ProductService(
            IProductRepository productRepository,
            IProductCategoryService productCategoryService,
            IProductImageRepository productImageRepository,
            ILogger<ProductService> logger,
            IMapper mapper,
            ApplicationDbContext context)
        {
            _productRepository = productRepository;
            _productCategoryService = productCategoryService;
            _productImageRepository = productImageRepository;
            _logger = logger;
            _mapper = mapper;
            _context = context;
        }

        public async Task AddProductAsync(AddProductDto productDto, List<int> categoryIds, List<ProductImageDto> productImagesDto)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                // Chuyển đổi dữ liệu DTO sang entity
                var product = _mapper.Map<Product>(productDto);
                var productImages = _mapper.Map<List<ProductImage>>(productImagesDto);

                // Thêm sản phẩm
                await _productRepository.AddAsync(product);

                // Gán danh mục
                await _productCategoryService.AssignProductToCategoriesAsync(product.Id, categoryIds);

                // Thêm hình ảnh
                await _productImageRepository.AddImageAsync(product.Id, productImages);

                // Commit giao dịch sau khi tất cả các thao tác hoàn thành
                await transaction.CommitAsync();
                _logger.LogInformation($"Product {product.Name} added successfully with categories and images.");
            }
            catch (Exception ex)
            {
                // Rollback nếu có lỗi
                await transaction.RollbackAsync();
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

        public async Task<PagingResult<ProductDto>> GetPagedProductsAsync(ProductPagingRequestDto requestDto)
        {
            _logger.LogInformation($"{nameof(GetPagedProductsAsync)}: input = {JsonSerializer.Serialize(requestDto)}");

            var result = await _productRepository.GetPagedProductsAsync(
                requestDto.PageNumber,
                requestDto.PageSize,
                requestDto.Keyword,
                requestDto.CategoryId,
                requestDto.MinPrice,
                requestDto.MaxPrice,
                requestDto.Sort
            );

            if (result.TotalItems == 0)
            {
                return new PagingResult<ProductDto>
                {
                    TotalItems = 0,
                    Items = new List<ProductDto>(),
                };
            }
            var productDtos = _mapper.Map<IEnumerable<ProductDto>>(result.Items);

            return new PagingResult<ProductDto>
            {
                TotalItems = result.TotalItems,
                Items = productDtos
            };
        }

        public async Task<IEnumerable<ProductDto>> GetAllProductsAsync()
        {
            var result = await _productRepository.GetAllAsync();
            if (result == null || !result.Any())
            {
                return new List<ProductDto>();
            }    
            var productDtos = _mapper.Map<IEnumerable<ProductDto>>(result);
            return productDtos;

        }

        public async Task<ProductDto> GetProductByIdAsync(int productId)
        {
            var product = await _productRepository.GetByIdAsync(productId);
            if (product == null)
            {
                throw new KeyNotFoundException($"Product with ID {productId} not found.");
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
