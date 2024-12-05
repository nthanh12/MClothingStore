using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Persistances;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class ProductCategoryRepository : IProductCategoryRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<ProductCategoryRepository> _logger;

        public ProductCategoryRepository(ApplicationDbContext context, ILogger<ProductCategoryRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task AddProductToCategoriesAsync(int productId, List<int> categoryIds)
        {
            var productCategories = categoryIds.Select(cId => new ProductCategory
            {
                ProductId = productId,
                CategoryId = cId
            }).ToList();

            if (productCategories.Any())
            {
                await _context.ProductCategories.AddRangeAsync(productCategories);
                await _context.SaveChangesAsync();
            }
        }
        public async Task RemoveProductFromCategoriesAsync(int productId, List<int> categoryIds)
        {
            var productCategories = await _context.ProductCategories
                .Where(pc => pc.ProductId == productId && categoryIds.Contains(pc.CategoryId))
                .ToListAsync();

            if (productCategories.Any())
            {
                _context.ProductCategories.RemoveRange(productCategories);
                await _context.SaveChangesAsync();
            }
        }
        public async Task AssignProductToCategoriesAsync(int productId, List<int> newCategoryIds)
        {
            await RemoveProductFromCategoriesAsync(productId, newCategoryIds);
            await AddProductToCategoriesAsync(productId, newCategoryIds);
        }

        public async Task<IEnumerable<ProductCategory>> GetCategoriesByProductIdAsync(int productId)
        {
            return await _context.ProductCategories
                .Where(pc => pc.ProductId == productId)
                .ToListAsync();
        }

        public async Task RemoveAllCategoriesFromProductAsync(int productId)
        {
            var productCategories = await _context.ProductCategories
                .Where(pc => pc.ProductId == productId)
                .ToListAsync();

            if (productCategories.Any())
            {
                _context.ProductCategories.RemoveRange(productCategories);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<int>> GetCategoriesToAddAsync(int productId, List<int> categoryIds)
        {
            // Lấy danh sách các CategoryId mà sản phẩm đã có từ bảng ProductCategories
            var existingCategoryIds = await _context.ProductCategories
                .Where(pc => pc.ProductId == productId)
                .Select(pc => pc.CategoryId)
                .ToListAsync();

            // Trả về danh sách CategoryId chưa có trong ProductCategories (loại bỏ các CategoryId đã có)
            return categoryIds.Except(existingCategoryIds).ToList();
        }

    }
}
