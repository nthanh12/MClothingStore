using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.EntityBase
{
    public interface IFullAudited : ICreatedBy, IModifiedBy, ISoftDelted
    {
    }

    public interface ISoftDelted
    {
        public bool Deleted { get; set; }
    }

    public interface ICreatedBy
    {
        public DateTime? CreatedDate { get; set; }
        public int? CreatedBy { get; set; }
    }

    public interface IModifiedBy
    {
        public DateTime? ModifiedDate { get; set; }
        public int? ModifiedBy { get; set; }
    }
}
