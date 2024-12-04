using Application.UserModules.DTOs.Product;
using Application.UserModules.DTOs.ProductImage;
using Domain.Entities;
using Shared.ApplicationBase.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UserModules.Abstracts
{
    public interface IProductService
    {
        Task AddProductAsync(AddProductDto productDto, List<int> categoryIds, List<AddProductImageDto> productImagesDto);
        Task UpdateProductAsync(UpdateProductDto productDto, List<int> newCategoryIds, List<UpdateProductImageDto> newProductImagesDto);
        Task DeleteProductAsync(int productId);
        Task<IEnumerable<ProductDto>> GetAllProductsAsync();
        Task<PagingResult<ProductDto>> GetPagedProductsAsync(ProductPagingRequestDto input);

        Task<ProductDto> GetProductByIdAsync(int productId);
    }
}
