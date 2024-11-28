using Domain.EntityBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class UserCustomer 
    { 
        public int ID { get; set; } 
        public int UserID { get; set; } 
        public int CustomerID { get; set; } 
        public User User { get; set; } 
        public Customer Customer { get; set; }
    }
}
