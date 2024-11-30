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

        public async Task AddAsync(ProductCategory productCategory)
        {
            await _context.ProductCategories.AddAsync(productCategory);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteByProductIdAsync(int productId)
        {
            var productCategories = await _context.ProductCategories
                .Where(pc => pc.ProductId == productId)
                .ToListAsync();

            _context.ProductCategories.RemoveRange(productCategories);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<ProductCategory>> GetByProductIdAsync(int productId)
        {
            return await _context.ProductCategories
                .Where(pc => pc.ProductId == productId)
                .ToListAsync();
        }

    }
}
