using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UserModules.DTOs.Product
{
    public class AddProductDto
    {
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string Color { get; set; } = null!;
        public decimal Price { get; set; }
    }
}
