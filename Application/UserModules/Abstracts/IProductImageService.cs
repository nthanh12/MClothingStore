using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.UserModules.Abstracts
{
    public interface IProductImageService
    {
        Task AddUpdateImagesToProductAsync(int productId, List<ProductImage> newImages);
        Task RemoveImageFromProductAsync(int productId, int imageId);
        Task RemoveAllImagesFromProductAsync(int productId);
        Task<IEnumerable<ProductImage>> GetImagesByProductIdAsync(int productId);
    }
}
