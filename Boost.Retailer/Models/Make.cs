using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Boost.Retail.Data.Models
{
    public class Make : BaseEntity
    {
        [Key]
        public string Code { get; set; } = string.Empty;

        public string Name { get; set; } = string.Empty;

        [DefaultValue(false)]
        public bool IsDeleted { get; set; } = false;
    }
}
