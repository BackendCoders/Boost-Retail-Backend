using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Boost.Admin.Data.Models
{
    public class UserSupplierProductCategoryBrand
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        
        public string UserId { get; set; }

        public int Category2Id { get; set; }
        public int Category3Id { get; set; }

        public int BrandId { get; set; }

        //public List<string> IgnoreMpns { get; set; } = new List<string>();
    }
}
