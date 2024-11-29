using Domain.Entities;
using Shared.ApplicationBase.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IProductRepository : IRepository<Product>
    {
        Task<IEnumerable<Product>> GetProductsByCategoryAsync(int categoryId);

        Task<IEnumerable<Product>> GetProductsByPriceRangeAsync(decimal minPrice, decimal maxPrice);

        Task<IEnumerable<Product>> GetProductsByKeywordAsync(string keyword);

        Task<PagingResult<Product>> GetPagedProductsAsync(int pageNumber, int pageSize, string? keyword = null, int? categoryId = null, decimal? minPrice = null, decimal? maxPrice = null);

    }

}
