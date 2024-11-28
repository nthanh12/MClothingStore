using Domain.EntityBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class UserRole : IFullAudited
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int RoleId { get; set; }
        public User User { get; set; } = new User();
        public Role Role { get; set; } = new Role();

        #region audit
        public DateTime? CreatedDate { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public int? ModifiedBy { get; set; }
        public bool Deleted { get; set; }
        #endregion
    }

}
