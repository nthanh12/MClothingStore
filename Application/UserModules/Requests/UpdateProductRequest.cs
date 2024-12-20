﻿using Application.UserModules.DTOs.Product;
using Application.UserModules.DTOs.ProductImage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UserModules.Requests
{
    public class UpdateProductRequest
    {
        public UpdateProductDto ProductDto { get; set; }
        public List<int> CategoryIds { get; set; }
        public List<UpdateProductImageDto> ProductImagesDto { get; set; }
    }
}
