using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Boost.Admin.DTOs
{
    public class CategoryMapRequestdto
    {
        public int Id { get; set; }

        public List<DynamicPropertyDto> DynamicProperties { get; set; }

    }

    public class FilterField
    {
        public string Value { get; set; }
        public string Filter { get; set; }
    }
    public class DynamicPropertyDto
    {
        public string ColumnName { get; set; }
        public object Value { get; set; }
        public string Filter { get; set; }
        public int ColumnType { get; set; }
    }


}
