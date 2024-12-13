using Application.UserModules.DTOs.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UserModules.DTOs.CartItem
{
    public class CartItemDto
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int Quantity { set; get; }
        public decimal UnitPrice { get; set; }
        public ProductDto Product { get; set; }
    }
}
