using Domain.EntityBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class TransactionStatus : IFullAudited
    {
        public int Id { get; set; }
        public int OrderID { get; set; }
        public int PaymentMethodID { get; set; }
        public DateTime TransactionDate { get; set; }
        public decimal Amount { get; set; }
        public TransactionState State { get; set; } // Trạng thái giao dịch (Pending, Cancelled, Failed, Completed)
        public string? ErrorMessage { get; set; } // Thông báo lỗi nếu có
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

    public enum TransactionState
    {
        Pending,    // Đang chờ xử lý
        Cancelled,  // Bị hủy
        Failed,     // Thất bại
        Completed   // Hoàn thành
    }
}