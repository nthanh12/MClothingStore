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
        Task AddProductAsync(Product product, List<ProductImage> productImages);
        Task UpdateProductAsync(Product product, List<ProductImage> productImages);
        Task DeleteProductAsync(int productId);
        Task<Product> GetProductWithImagesAsync(int productId);
        Task<IEnumerable<Product>> GetAllProductsAsync();
    }
}
