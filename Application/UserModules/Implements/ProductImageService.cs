using Application.UserModules.Abstracts;
using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Repositories;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UserModules.Implements
{
    public class ProductImageService : IProductImageService
    {
        private readonly IProductImageRepository _productImageRepository;
        private readonly ILogger<ProductImageService> _logger;

        public ProductImageService(
            IProductImageRepository productImageRepository,
            ILogger<ProductImageService> logger)
        {
            _productImageRepository = productImageRepository;
            _logger = logger;
        }

        public async Task AddUpdateImagesToProductAsync(int productId, List<ProductImage> newImages)
        {
            try
            {
                // Xóa tất cả ảnh cũ của sản phẩm
                await _productImageRepository.DeleteImagesByProductIdAsync(productId);

                // Thêm tất cả ảnh mới vào sản phẩm
                await _productImageRepository.AddImageAsync(productId, newImages);

                // Log thông báo thành công
                _logger.LogInformation($"Successfully replaced all images for product {productId}.");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error replacing images for product {productId}: {ex.Message}");
                throw;
            }
        }

        public async Task<IEnumerable<ProductImage>> GetImagesByProductIdAsync(int productId)
        {
            try
            {
                return await _productImageRepository.GetImagesByProductIdAsync(productId);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error retrieving images for product {productId}: {ex.Message}");
                throw;
            }
        }

        public Task RemoveAllImagesFromProductAsync(int productId)
        {
            throw new NotImplementedException();
        }

        public async Task RemoveImageFromProductAsync(int productId, int imageId)
        {
            try
            {
                // Xóa ảnh trực tiếp nếu có ảnh với imageId
                await _productImageRepository.DeleteImageAsync(productId, imageId);
                _logger.LogInformation($"Successfully removed image with ID {imageId} from product {productId}.");

            }
            catch (Exception ex)
            {
                _logger.LogError($"Error removing image with ID {imageId} from product {productId}: {ex.Message}");
                throw;
            }
        }



    }
}
