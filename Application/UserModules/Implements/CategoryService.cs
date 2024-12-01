using Application.UserModules.Abstracts;
using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Persistances;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UserModules.Implements
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly ILogger<CategoryService> _logger;

        public CategoryService(ICategoryRepository categoryRepository, ILogger<CategoryService> logger)
        {
            _categoryRepository = categoryRepository;
            _logger = logger;
        }

        public async Task AddCategoryAsync(Category category)
        {
            try
            {
                var existingCategory = await _categoryRepository.GetByNameAsync(category.Name);
                if (existingCategory != null)
                {
                    _logger.LogWarning($"Category with name '{category.Name}' already exists.");
                    throw new InvalidOperationException($"Category with name '{category.Name}' already exists.");
                }

                await _categoryRepository.AddAsync(category);
                _logger.LogInformation($"Category '{category.Name}' added successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error adding category '{category.Name}'");
                throw;
            }
        }


        public async Task DeleteCategoryAsync(int categoryId)
        {
            try
            {
                await _categoryRepository.DeleteAsync(categoryId);
                _logger.LogInformation($"Category with ID {categoryId} deleted successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error deleting category with ID {categoryId}");
                throw;
            }
        }

        public async Task<IEnumerable<Category>> GetAllCategoriesAsync()
        {
            try
            {
                var categories = await _categoryRepository.GetAllAsync();
                _logger.LogInformation("Retrieved all categories successfully");
                return categories;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving all categories");
                throw;
            }
        }


        public async Task<Category> GetCategoryByIdAsync(int categoryId)
        {
            try
            {
                var category = await _categoryRepository.GetByIdAsync(categoryId);
                if (category == null)
                {
                    _logger.LogWarning($"Category with ID {categoryId} not found.");
                    return null;}

                _logger.LogInformation($"Retrieved category with ID {categoryId} successfully.");
                return category;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error retrieving category with ID {categoryId}");
                throw;
            }
        }


        public async Task UpdateCategoryAsync(Category category)
        {
            try
            {
                var existingCategory = await _categoryRepository.GetByIdAsync(category.Id);
                if (existingCategory == null)
                {
                    _logger.LogWarning($"Category with ID {category.Id} not found. Cannot update.");
                    throw new KeyNotFoundException($"Category with ID {category.Id} not found.");
                }

                await _categoryRepository.UpdateAsync(category);
                _logger.LogInformation($"Category with ID {category.Id} updated successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error updating category with ID {category.Id}");
                throw;
            }
        }

    }
}
