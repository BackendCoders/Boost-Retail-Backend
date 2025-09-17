using Swashbuckle.AspNetCore.Annotations;

namespace Boost.Retail.Data.Models
{

    public class ProductCategory : BaseEntity
    {
        public string Code { get; set; } = string.Empty;

        public string Name { get; set; } = string.Empty;

        public bool? A { get; set; } = true;

        public bool? B { get; set; } = true;

        public bool? C { get; set; } = true;

        public bool Major { get; set; } = false;

        public bool IsDeleted { get; set; } = false;
    }
}
