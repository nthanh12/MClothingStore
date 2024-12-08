using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UserModules.DTOs.Product
{
    public class ProductForSaleDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string Color { get; set; } = null!;
        public decimal Price { get; set; }
        public ICollection<string> ImageUrls { get; set; } = new List<string>();
    }
}
