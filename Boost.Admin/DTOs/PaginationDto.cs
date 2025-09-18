using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Boost.Admin.DTOs
{
    public class PaginationDto
    {
        public int PageNumber { get; set; } = 1;   // default first page
        public int PageSize { get; set; } = 10;    // default size
    }
}
