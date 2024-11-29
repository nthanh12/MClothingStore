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
        public async Task AddImageAsync(int productId, ProductImage productImage)
        {
            var product = await _context.Set<Product>().FindAsync(productId);
            if (product == null)
            {
                throw new Exception("Sản phẩm không tồn tại.");
            }

            productImage.ProductId = productId;
            await _context.Set<ProductImage>().AddAsync(productImage);
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

        public async Task<ProductImage> GetImageByIdAsync(int productId, int imageId)
        {
            var product = await _context.Set<Product>().FindAsync(productId);
            if (product == null)
            {
                throw new Exception("Sản phẩm không tồn tại.");
            }

            return await _context.Set<ProductImage>().FirstOrDefaultAsync(pi => pi.ProductId == productId && pi.Id == imageId);

        }

        public async Task<IEnumerable<ProductImage>> GetImagesByProductIdAsync(int productId)
        {
            var product = await _context.Set<Product>().FindAsync(productId);
            if (product == null)
            {
                throw new Exception("Sản phẩm không tồn tại.");
            }

            return await _context.Set<ProductImage>().Where(pi => pi.ProductId == productId).ToListAsync();

        }

        public async Task UpdateImageAsync(int productId, ProductImage productImage)
        {
            var product = await _context.Set<Product>().FindAsync(productId);
            if (product == null)
            {
                throw new Exception("Sản phẩm không tồn tại.");
            }

            var existingImage = await _context.Set<ProductImage>().FirstOrDefaultAsync(pi => pi.ProductId == productId && pi.Id == productImage.Id);
            if (existingImage == null)
            {
                throw new Exception("Hình ảnh không tồn tại.");
            }

            existingImage.ImageUrl = productImage.ImageUrl;
            await _context.SaveChangesAsync();
        }
    }
}
