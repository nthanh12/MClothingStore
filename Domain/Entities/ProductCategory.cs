using Domain.EntityBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class ProductCategory : IFullAudited
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int CategoryId { get; set; }
        public Product Product { get; set; } = new Product();
        public Category Category { get; set; } = new Category();

        #region audit
        public DateTime? CreatedDate { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public int? ModifiedBy { get; set; }
        public bool Deleted { get; set; }
        #endregion
    }

}
