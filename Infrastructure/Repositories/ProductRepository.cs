using Domain.Entities;
using Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Shared.ApplicationBase.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Persistances
{
    public class ProductRepository : IProductRepository
    {
        private readonly ApplicationDbContext _context;
        public ProductRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        // Thêm sản phẩm mới
        public async Task AddAsync(Product product)
        {
            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();
        }

        // Xóa sản phẩm theo ID
        public async Task DeleteAsync(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product != null)
            {
                _context.Products.Remove(product);
                await _context.SaveChangesAsync();
            }
        }

        // Lấy tất cả sản phẩm
        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            return await _context.Products.ToListAsync();
        }

        // Lấy sản phẩm theo ID
        public async Task<Product> GetByIdAsync(int id)
        {
            return await _context.Products.FindAsync(id);
        }

        // Cập nhật sản phẩm
        public async Task UpdateAsync(Product product)
        {
            _context.Products.Update(product);
            await _context.SaveChangesAsync();
        }

        // Tìm kiếm sản phẩm theo từ khóa
        public async Task<List<Product>> SearchAsync(string searchTerm)
        {
            return await _context.Products
                .Where(p => p.Name.Contains(searchTerm) || p.Description.Contains(searchTerm))
                .ToListAsync();
        }
      
        public async Task<PagingResult<Product>> GetPagedProductsAsync(
            int pageNumber,
            int pageSize,
            string? keyword = null,
            int? categoryId = null,
            decimal? minPrice = null,
            decimal? maxPrice = null,
            List<string> sort = null) 
        {
            var query = _context.Products.AsQueryable();

            // keyword
            if (!string.IsNullOrEmpty(keyword))
            {
                query = query.Where(p => p.Name.Contains(keyword) || p.Description.Contains(keyword));
            }

            // categoryId
            if (categoryId.HasValue)
            {
                query = query.Where(p => p.ProductCategories.Any(pc => pc.CategoryId == categoryId));
            }

            // Price
            if (minPrice.HasValue)
            {
                query = query.Where(p => p.Price >= minPrice.Value);
            }
            if (maxPrice.HasValue)
            {
                query = query.Where(p => p.Price <= maxPrice.Value);
            }

            // dynamic sorting
            if (sort != null && sort.Any())
            {
                foreach (var sortCondition in sort)
                {
                    var parts = sortCondition.Split(' ');   
                    var property = parts[0];                
                    var direction = parts.Length > 1 && parts[1].ToLower() == "desc" ? "desc" : "asc";  

                    if (direction == "asc")
                        query = query.OrderBy(p => EF.Property<object>(p, property));
                    else
                        query = query.OrderByDescending(p => EF.Property<object>(p, property));
                }
            }
            // total record
            var totalItems = await query.CountAsync();
            // paged
            var items = await query.Skip((pageNumber - 1) * pageSize)
                                   .Take(pageSize)
                                   .ToListAsync();

            return new PagingResult<Product>
            {
                TotalItems = totalItems,   
                Items = items             
            };
        }

        // Lấy sản phẩm theo danh mục
        public async Task<IEnumerable<Product>> GetProductsByCategoryAsync(int categoryId)
        {
            return await _context.Products
                .Where(p => p.ProductCategories.Any(pc => pc.CategoryId == categoryId))
                .ToListAsync();
        }

        // Lấy sản phẩm theo từ khóa
        public async Task<IEnumerable<Product>> GetProductsByKeywordAsync(string keyword)
        {
            return await _context.Products
                .Where(p => p.Name.Contains(keyword) || p.Description.Contains(keyword))
                .ToListAsync();
        }

        // Lấy sản phẩm trong khoảng giá
        public async Task<IEnumerable<Product>> GetProductsByPriceRangeAsync(decimal minPrice, decimal maxPrice)
        {
            return await _context.Products
                .Where(p => p.Price >= minPrice && p.Price <= maxPrice)
                .ToListAsync();
        }
    }
}
