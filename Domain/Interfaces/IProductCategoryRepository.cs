using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IProductCategoryRepository
    {
        Task AssignCategoriesAsync(int productId, List<int> categoryIds);

        Task RemoveCategoriesAsync(int productId, List<int> categoryIds);

        Task<IEnumerable<ProductCategory>> GetCategoriesByProductIdAsync(int productId);

        Task RemoveProductFromCategoryAsync(int productId, int categoryId);
    }
}
