using Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IProductCategoryRepository
    {
        Task AddProductToCategoriesAsync(int productId, List<int> categoryIds);
        Task RemoveProductFromCategoriesAsync(int productId, List<int> categoryIds);
        Task AssignProductToCategoriesAsync(int productId, List<int> newCategoryIds);
        Task<IEnumerable<ProductCategory>> GetCategoriesByProductIdAsync(int productId);
        Task RemoveAllCategoriesFromProductAsync(int productId);
        Task<List<int>> GetCategoriesToAddAsync(int productId, List<int> categoryIds);
    }
}
