using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Boost.Retail.Data.Models
{
    public class UserProduct
    {
        [Key]
        public int Id { get; set; }
        public string? EPN { get; set; } = string.Empty;

        [Required]
        public string MPN { get; set; }
        public string? Barcode { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string? GroupName { get; set; } = string.Empty;

        [Required]
        public string Brand { get; set; }

        [Required]
        public string Supplier { get; set; }

        public string Colour { get; set; }
        public string Size { get; set; }
        public string? Weight { get; set; } = string.Empty;

        public List<string>? Images { get; set; }

        public string? VideoUrl { get; set; }
        public string? SupplierDetailsUrl { get; set; }
        public string ShortDescription { get; set; }
        public string LongDescription { get; set; }

        [Precision(18, 2)]
        public double? Cost { get; set; }

        [Precision(18, 2)]
        public double Price { get; set; }

        [Precision(18, 2)]
        public double? PromoPrice { get; set; } = 0;
        public DateTime? PromoStart { get; set; }
        public DateTime? PromoEnd { get; set; }

        [Precision(18, 2)]
        public double VatRate { get; set; }
        public string Category1 { get; set; }
        public string Category2 { get; set; }
        public string Category3 { get; set; }
        public string GeometryJson { get; set; }
        public string SpecificationsJson { get; set; }

        public int? Year { get; set; }
    }
}
