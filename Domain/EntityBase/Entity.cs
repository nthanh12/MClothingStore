using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.EntityBase
{
    public class Entity<TKey> : IEntity<TKey>
    {
        public TKey Id { get; set; }
    }

    public interface IEntity<TKey>
    {
        TKey Id { get; set; }
    }

    public class Entity : Entity<int>
    {
    }
}
