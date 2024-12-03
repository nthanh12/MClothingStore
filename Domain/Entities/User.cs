using Domain.EntityBase;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class User : IFullAudited
    {
        public int Id { get; set; }

        [MaxLength(50)]
        public string Username { get; set; } = null!;

        [MaxLength(512)]
        public string Password { get; set; } = null!;
        public int Status { get; set; }
        public int UserType { get; set; }

        public List<UserRole> UserRoles { get; set; } = new();

        #region audit
        public DateTime? CreatedDate { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public int? ModifiedBy { get; set; }
        public bool Deleted { get; set; }
        #endregion
    }

}
