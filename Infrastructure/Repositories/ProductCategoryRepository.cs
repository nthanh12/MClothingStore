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
    public class ProductCategoryRepository : IProductCategoryRepository
    {
        private readonly ApplicationDbContext _context;

        public ProductCategoryRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AssignCategoriesAsync(int productId, List<int> categoryIds)
        {
            var existingCategories = await _context.ProductCategories
                .Where(pc => pc.ProductId == productId)
                .ToListAsync();

            var newCategories = categoryIds.Where(c => !existingCategories.Any(ec => ec.CategoryId == c)).ToList();

            var productCategories = newCategories.Select(categoryId => new ProductCategory
            {
                ProductId = productId,
                CategoryId = categoryId
            }).ToList();

            await _context.ProductCategories.AddRangeAsync(productCategories);
            await _context.SaveChangesAsync();
        }

        public async Task RemoveCategoriesAsync(int productId, List<int> categoryIds)
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

        public async Task RemoveProductFromCategoryAsync(int productId, int categoryId)
        {
            var productCategory = await _context.ProductCategories
                .FirstOrDefaultAsync(pc => pc.ProductId == productId && pc.CategoryId == categoryId);

            if (productCategory != null)
            {
                _context.ProductCategories.Remove(productCategory);
                await _context.SaveChangesAsync();
            }
        }
    }
}
