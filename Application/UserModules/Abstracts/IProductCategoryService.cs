using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Application.UserModules.Abstracts
{
    public interface IProductCategoryService
    {
        Task AssignProductToCategoriesAsync(int productId, List<int> categoryIds);

        Task RemoveProductFromCategoriesAsync(int productId, List<int> categoryIds);

        Task RemoveProductFromAllCategoriesAsync(int productId);
    }
}
