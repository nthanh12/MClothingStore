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

        public async Task AssignProductToCategoriesAsync(int productId, List<int> categoryIds)
        {
            var categories = await _context.Categories
                .Where(c => categoryIds.Contains(c.Id))
                .ToListAsync();

            if (categories.Any(c => string.IsNullOrEmpty(c.Name)))
            {
                throw new InvalidOperationException("One or more categories have an invalid Name.");
            }

            var existingProductCategories = await _context.ProductCategories
                .Where(pc => pc.ProductId == productId && categoryIds.Contains(pc.CategoryId))
                .ToListAsync();

            var existingCategoryIds = existingProductCategories.Select(pc => pc.CategoryId).ToList();
            var newCategoryIds = categoryIds.Except(existingCategoryIds).ToList();

            if (!newCategoryIds.Any())
            {
                return;
            }

            var productCategories = newCategoryIds.Select(cId => new ProductCategory
            {
                ProductId = productId,
                CategoryId = cId
            }).ToList();

            // Ghi nhật ký trước khi thêm vào cơ sở dữ liệu
            foreach (var productCategory in productCategories)
            {
                _logger.LogInformation($"Before Add: ProductId: {productCategory.ProductId}, CategoryId: {productCategory.CategoryId}");
            }

            await _context.ProductCategories.AddRangeAsync(productCategories);
            await _context.SaveChangesAsync();

            // Ghi nhật ký sau khi lưu vào cơ sở dữ liệu
            foreach (var productCategory in productCategories)
            {
                _logger.LogInformation($"After Save: ProductId: {productCategory.ProductId}, CategoryId: {productCategory.CategoryId}");
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
