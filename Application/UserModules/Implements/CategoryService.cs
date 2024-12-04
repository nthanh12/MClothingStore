using Application.UserModules.Abstracts;
using Application.UserModules.DTOs.Category;
using Application.UserModules.DTOs.Product;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Persistances;
using Microsoft.Extensions.Logging;
using Shared.Consts.Exceptions;
using Shared.Exceptions;
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
        private readonly IMapper _mapper;

        public CategoryService(ICategoryRepository categoryRepository, ILogger<CategoryService> logger, IMapper mapper)
        {
            _categoryRepository = categoryRepository;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task AddCategoryAsync(AddCategoryDto categoryDto)
        {
            try
            {
                var category = _mapper.Map<Category>(categoryDto);

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
                _logger.LogError(ex, $"Error adding category '{categoryDto.Name}'");
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

        public async Task<IEnumerable<CategoryDto>> GetAllCategoriesAsync()
        {
            try
            {
                var result = await _categoryRepository.GetAllAsync();
                if (result == null || !result.Any())
                {
                    return new List<CategoryDto>();
                }
                var categoryDto = _mapper.Map<IEnumerable<CategoryDto>>(result);
                return categoryDto;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving all categories");
                throw;
            }
        }


        public async Task<CategoryDto> GetCategoryByIdAsync(int categoryId)
        {
            try
            {
                var category = await _categoryRepository.GetByIdAsync(categoryId);
                if (category == null)
                {
                    _logger.LogWarning($"Category with ID {categoryId} not found.");
                    throw new UserFriendlyException(ErrorCode.CategoryNotFound);
                }
                _logger.LogInformation($"Retrieved category with ID {categoryId} successfully.");
                var categoryDto = _mapper.Map<CategoryDto>(category);

                return categoryDto;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error retrieving category with ID {categoryId}");
                throw;
            }
        }


        public async Task UpdateCategoryAsync(UpdateCategoryDto categoryDto)
        {
            try
            {
                var existingCategory = await _categoryRepository.GetByIdAsync(categoryDto.Id);
                if (existingCategory == null)
                {
                    _logger.LogWarning($"Category with ID {categoryDto.Id} not found. Cannot update.");
                    throw new KeyNotFoundException($"Category with ID {categoryDto.Id} not found.");
                }

                // Cập nhật các thuộc tính cần thay đổi
                _mapper.Map(categoryDto, existingCategory);

                await _categoryRepository.UpdateAsync(existingCategory);
                _logger.LogInformation($"Category with ID {existingCategory.Id} updated successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error updating category with ID {categoryDto.Id}");
                throw;
            }
        }

    }
}
