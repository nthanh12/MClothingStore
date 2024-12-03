using Application.UserModules.DTOs.Product;
using Application.UserModules.DTOs.ProductImage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UserModules.Requests
{
    public class AddProductRequest
    {
        public AddProductDto ProductDto { get; set; }
        public List<int> CategoryIds { get; set; }
        public List<ProductImageDto> ProductImagesDto { get; set; }
    }
}
