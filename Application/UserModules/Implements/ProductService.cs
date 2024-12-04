using Application.UserModules.Abstracts;
using Application.UserModules.DTOs.Product;
using Application.UserModules.DTOs.ProductImage;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Persistances;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Shared.ApplicationBase.Common;
using Shared.Consts.Exceptions;
using Shared.Exceptions;
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
        private readonly IProductImageService _productImageService;
        private readonly ILogger<ProductService> _logger;
        private readonly IMapper _mapper;
        public readonly ApplicationDbContext _context;

        public ProductService(
            IProductRepository productRepository,
            IProductCategoryService productCategoryService,
            IProductImageService productImageService,
            ILogger<ProductService> logger,
            IMapper mapper,
            ApplicationDbContext context)
        {
            _productRepository = productRepository;
            _productCategoryService = productCategoryService;
            _productImageService = productImageService;
            _logger = logger;
            _mapper = mapper;
            _context = context;
        }

        public async Task AddProductAsync(AddProductDto productDto, List<int> categoryIds, List<AddProductImageDto> productImagesDto)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                // Chuyển đổi dữ liệu DTO thành entity
                var product = _mapper.Map<Product>(productDto);
                var productImages = _mapper.Map<List<ProductImage>>(productImagesDto);

                //await _context.Products.AddAsync(product);
                //await _context.SaveChangesAsync();

                //var productId = product.Id;

                //categoryIds.ForEach(item =>
                //{
                //    var newProductCategory = new ProductCategory()
                //    {
                //        ProductId = productId,
                //        CategoryId = item,
                //    };
                //    _context.ProductCategories.Add(newProductCategory);
                //});

                //await _context.SaveChangesAsync();

                //Thêm sản phẩm vào cơ sở dữ liệu
                await _productRepository.AddAsync(product);
                await _context.SaveChangesAsync(); 

                // Kiểm tra giá trị ProductId sau khi lưu
                _logger.LogInformation($"Product added with ID: {product.Id}");

                // Gán sản phẩm vào các danh mục
                await _productCategoryService.AssignProductToCategoriesAsync(product.Id, categoryIds);

                // Gán hình ảnh cho sản phẩm
                await _productImageService.AddUpdateImagesToProductAsync(product.Id, productImages);

                // Commit transaction
                await transaction.CommitAsync();

                _logger.LogInformation($"Product {product.Name} added successfully with categories and images.");
            }
            catch (Exception ex)
            {
                // Rollback transaction khi có lỗi
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
                await _productCategoryService.RemoveAllCategoriesFromProductAsync(productId);

                // Xóa tất cả các ảnh liên quan đến sản phẩm
                await _productImageService.RemoveAllImagesFromProductAsync(productId);

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
            try
            {
                var result = await _productRepository.GetAllAsync();
                if (result == null || !result.Any())
                {
                    return new List<ProductDto>();
                }
                var productDtos = _mapper.Map<IEnumerable<ProductDto>>(result);
                return productDtos;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving all products");
                throw;
            }

        }

        public async Task<ProductDto> GetProductByIdAsync(int productId)
        {
            try
            {
                var product = await _productRepository.GetByIdAsync(productId);
                if (product == null)
                {
                    _logger.LogWarning($"Product with ID {productId} not found.");
                    throw new UserFriendlyException(ErrorCode.ProductNotFound);
                }

                var productDto = _mapper.Map<ProductDto>(product);
                return productDto;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error retrieving product by ID: {productId}.");
                throw;
            }
        }

        public async Task UpdateProductAsync(UpdateProductDto productDto, List<int> newCategoryIds, List<UpdateProductImageDto> newProductImagesDto)
        {
            try
            {
                var productImages = _mapper.Map<List<ProductImage>>(newProductImagesDto);

                // Lấy sản phẩm cũ từ repository
                var existingProduct = await _productRepository.GetByIdAsync(productDto.Id);
                if (existingProduct == null)
                {
                    _logger.LogWarning($"Product with ID {productDto.Id} not found. Cannot update.");
                    throw new KeyNotFoundException($"Product with ID {productDto.Id} not found.");
                }

                // Áp dụng các thay đổi từ DTO vào đối tượng sản phẩm hiện tại
                _mapper.Map(productDto, existingProduct);

                // Cập nhật thông tin sản phẩm
                await _productRepository.UpdateAsync(existingProduct);

                // Cập nhật danh mục cho sản phẩm
                await _productCategoryService.AssignProductToCategoriesAsync(existingProduct.Id, newCategoryIds);

                // Nếu có thay đổi ảnh, xóa ảnh cũ và thêm ảnh mới
                if (newProductImagesDto != null && newProductImagesDto.Any())
                {
                    await _productImageService.AddUpdateImagesToProductAsync(existingProduct.Id, productImages);
                }

                _logger.LogInformation($"Product {existingProduct.Name} updated successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error updating product {productDto.Name}: {ex.Message}");
                throw;
            }
        }
    }
}
