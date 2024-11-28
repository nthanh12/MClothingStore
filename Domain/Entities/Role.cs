using Domain.EntityBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Role : IFullAudited
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;

        public ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();

        #region audit
        public DateTime? CreatedDate { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public int? ModifiedBy { get; set; }
        public bool Deleted { get; set; }
        #endregion
    }

}
