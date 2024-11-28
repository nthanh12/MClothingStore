using Domain.EntityBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Payment : IFullAudited
    {
        public int ID { get; set; }
        public int OrderID { get; set; }
        public int PaymentMethodID { get; set; }
        public DateTime PaymentDate { get; set; }
        public decimal Amount { get; set; }
        public Order Order { get; set; }
        public PaymentMethod PaymentMethod { get; set; }

        #region audit
        public DateTime? CreatedDate { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public int? ModifiedBy { get; set; }
        public bool Deleted { get; set; }
        #endregion
    }
}
