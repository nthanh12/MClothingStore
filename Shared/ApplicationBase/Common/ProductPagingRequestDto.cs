using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.ApplicationBase.Common
{
    public class ProductPagingRequestDto : PagingRequestBaseDto
    {
        [FromQuery(Name = "categoryId")]
        public int? CategoryId { get; set; }

        [FromQuery(Name = "minPrice")]
        public decimal? MinPrice { get; set; }

        [FromQuery(Name = "maxPrice")]
        public decimal? MaxPrice { get; set; }


    }
}
