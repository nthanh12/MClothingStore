using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IProductImageRepository
    {
        Task<IEnumerable<ProductImage>> GetImagesByProductIdAsync(int productId);
        Task<ProductImage> GetImageByIdAsync(int productId, int imageId);
        Task AddImageAsync(int productId, List<ProductImage> productImage);
        Task UpdateImageAsync(int productId, List<ProductImage> productImage);
        Task DeleteImageAsync(int productId, int imageId);
    }
}
