using Domain.EntityBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class ProductImage : IFullAudited
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string? ImageUrl { get; set; }
        public Product Product { get; set; } = new Product();

        #region audit
        public DateTime? CreatedDate { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public int? ModifiedBy { get; set; }
        public bool Deleted { get; set; }
        #endregion
    }

}
