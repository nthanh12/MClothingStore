using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IProductCategoryRepository
    {
        Task AssignProductToCategoriesAsync(int productId, List<int> categoryIds);
        Task RemoveProductFromCategoriesAsync(int productId, List<int> categoryIds);     
        Task<IEnumerable<ProductCategory>> GetCategoriesByProductIdAsync(int productId);
        Task<List<int>> GetCategoriesToAddAsync(int productId, List<int> categoryIds);
        Task RemoveAllCategoriesFromProductAsync(int productId);
    }
}
