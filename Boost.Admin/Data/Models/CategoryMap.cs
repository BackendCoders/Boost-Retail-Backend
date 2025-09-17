using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Boost.Admin.Data.Models
{
    public class CategoryMap
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public BrandType Brand { get; set; }

        [Required]
        public string BrandName { get; set; }
        [Required]
        public string Model { get; set; }
        [Required]
        public string Category1 { get; set; }
        [Required]
        public string Category2 { get; set; }
        [Required]
        public string Category3 { get; set; }
    }
}
