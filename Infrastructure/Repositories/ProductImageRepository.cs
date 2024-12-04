using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Persistances;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class ProductImageRepository : IProductImageRepository
    {
        private readonly ApplicationDbContext _context;

        public ProductImageRepository(ApplicationDbContext context) 
        {
            _context = context;
        }
        public async Task AddImageAsync(int productId, List<ProductImage> productImages)
        {
            if (productImages == null || !productImages.Any())
            {
                throw new ArgumentException("Danh sách hình ảnh không được rỗng.", nameof(productImages));
            }

            var productExists = await _context.Set<Product>().AnyAsync(p => p.Id == productId);
            if (!productExists)
            {
                throw new Exception("Sản phẩm không tồn tại.");
            }

            // Gán ProductId cho mỗi hình ảnh trong danh sách
            productImages.ForEach(image => image.ProductId = productId);

            await _context.Set<ProductImage>().AddRangeAsync(productImages);
            await _context.SaveChangesAsync();
        }


        public async Task DeleteImageAsync(int productId, int imageId)
        {
            var product = await _context.Set<Product>().FindAsync(productId);
            if (product == null)
            {
                throw new Exception("Sản phẩm không tồn tại.");
            }

            var productImage = await _context.Set<ProductImage>().FirstOrDefaultAsync(pi => pi.ProductId == productId && pi.Id == imageId);
            if (productImage == null)
            {
                throw new Exception("Hình ảnh không tồn tại.");
            }

            _context.Set<ProductImage>().Remove(productImage);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteImagesByProductIdAsync(int productId)
        {
            var imagesToDelete = await _context.ProductImages
                                                 .Where(img => img.ProductId == productId)
                                                 .ToListAsync();
            if (!imagesToDelete.Any())
            {
                return;
            }

            _context.ProductImages.RemoveRange(imagesToDelete);

            await _context.SaveChangesAsync();
        }


        public async Task<ProductImage> GetImageByIdAsync(int productId, int imageId)
        {
            var product = await _context.Set<Product>().FindAsync(productId);
            if (product == null)
            {
                throw new Exception("Sản phẩm không tồn tại.");
            }

            return await _context.Set<ProductImage>().FirstOrDefaultAsync(pi => pi.ProductId == productId && pi.Id == imageId);

        }

        public async Task UpdateImageAsync(int productId, List<ProductImage> productImages)
        {
            if (productImages == null || !productImages.Any())
            {
                throw new ArgumentException("Danh sách hình ảnh không được rỗng.", nameof(productImages));
            }

            var existingImages = await _context.Set<ProductImage>()
                .Where(pi => pi.ProductId == productId && productImages.Select(p => p.Id).Contains(pi.Id))
                .ToListAsync();

            if (existingImages.Count != productImages.Count)
            {
                throw new Exception("Một hoặc nhiều hình ảnh không tồn tại hoặc không thuộc sản phẩm này.");
            }

            foreach (var updatedImage in productImages)
            {
                var existingImage = existingImages.FirstOrDefault(ei => ei.Id == updatedImage.Id);
                if (existingImage != null)
                {
                    existingImage.ImageUrl = updatedImage.ImageUrl;
                }
            }

            _context.Set<ProductImage>().UpdateRange(existingImages);
            await _context.SaveChangesAsync();
        }
        public async Task<IEnumerable<ProductImage>> GetImagesByProductIdAsync(int productId)
        {
            var productImages = await _context.Set<ProductImage>()
                                               .Where(pi => pi.ProductId == productId)
                                               .ToListAsync();
            return productImages;
        }


    }
}
