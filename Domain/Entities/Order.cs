using Domain.EntityBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Order : IFullAudited
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }
        public ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
        public ICollection<Payment> Payments { get; set; } = new List<Payment>(); // Điều hướng đến Payment

        public Customer Customer { get; set; }

        #region audit
        public DateTime? CreatedDate { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public int? ModifiedBy { get; set; }
        public bool Deleted { get; set; }
        #endregion
    }
}
