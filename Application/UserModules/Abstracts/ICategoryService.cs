using Application.UserModules.DTOs.Category;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UserModules.Abstracts
{
    public interface ICategoryService
    {
        Task<IEnumerable<CategoryDto>> GetAllCategoriesAsync();
        Task<CategoryDto> GetCategoryByIdAsync(int categoryId);
        Task AddCategoryAsync(AddCategoryDto category);
        Task UpdateCategoryAsync(UpdateCategoryDto category);
        Task DeleteCategoryAsync(int categoryId);
    }
}
