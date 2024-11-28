using Domain.EntityBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Product : IFullAudited
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string Color { get; set; }
        public decimal Price { get; set; }

        public ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
        public ICollection<ProductImage> ProductImages { get; set; } = new List<ProductImage>();
        public ICollection<ProductCategory> ProductCategories { get; set; } = new List<ProductCategory>();

        #region audit
        public DateTime? CreatedDate { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public int? ModifiedBy { get; set; }
        public bool Deleted { get; set; }
        #endregion
    }

}
