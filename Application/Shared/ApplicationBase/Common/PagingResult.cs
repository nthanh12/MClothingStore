using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Shared.ApplicationBase.Common
{
    public class PagingResult<T>
    {
        public IEnumerable<T> Items { get; set; } = Enumerable.Empty<T>();
        public int TotalItems { get; set; }
    }
}
