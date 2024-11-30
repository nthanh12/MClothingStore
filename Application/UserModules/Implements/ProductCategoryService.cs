using Application.UserModules.Abstracts;
using Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UserModules.Implements
{
    public class ProductCategoryService : IProductCategoryService
    {
        private readonly IProductCategoryRepository _productCategoryRepository;

        public ProductCategoryService(IProductCategoryRepository productCategoryRepository)
        {
            _productCategoryRepository = productCategoryRepository;
        }
        public async Task AssignProductToCategoriesAsync(int productId, List<int> categoryIds)
        {
            await _productCategoryRepository.AssignCategoriesAsync(productId, categoryIds);
        }

        public async Task RemoveProductFromCategoriesAsync(int productId, List<int> categoryIds)
        {
            await _productCategoryRepository.RemoveCategoriesAsync(productId, categoryIds);
        }
    }
}
