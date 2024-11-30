using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UserModules.Abstracts
{
    public interface IProductService
    {
        Task AddProductAsync(Product product, List<int> categoryIds, List<ProductImage> productImages);
        Task UpdateProductAsync(Product product, List<int> newCategoryIds, List<ProductImage> newProductImages);
        Task DeleteProductAsync(int productId);
        Task<IEnumerable<Product>> GetAllProductsAsync();
        Task<Product> GetProductByIdAsync(int productId);
    }
}
