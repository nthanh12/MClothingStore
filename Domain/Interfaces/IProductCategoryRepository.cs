using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IProductCategoryRepository
    {
        Task AssignCategoriesAsync(int productId, List<int> categoryIds); 
        Task RemoveCategoriesAsync(int productId, List<int> categoryIds);

    }
}
