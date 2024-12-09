using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UserModules.DTOs.Customer
{
    public class RegisterCustomerDto
    {
        public string FirstName { get; set; } = null!; 
        public string LastName { get; set; } = null!; 
        public string Email { get; set; } = null!; 
        public string Phone { get; set; } = null!; 
        public string Address { get; set; } = null!; 
    }
}
