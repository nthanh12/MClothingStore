using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UserModules.DTOs.Cart
{
    public class AddCartDto
    {
        public int UserId { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
