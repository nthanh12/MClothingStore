using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.UserModules.DTOs.Product
{
    public class ProductDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string Color { get; set; }
        public decimal Price { get; set; }
        public ICollection<string> ImageUrls { get; set; } = new List<string>();

        #region Audit
        public DateTime? CreatedDate { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public int? ModifiedBy { get; set; }
        public bool Deleted { get; set; } 
        #endregion
    }
}
