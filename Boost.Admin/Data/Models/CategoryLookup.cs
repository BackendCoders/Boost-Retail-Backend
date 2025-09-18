using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Boost.Admin.Data.Models
{
    public class CategoryLookup
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public BrandType SupplierFeed { get; set; }

        [Required]
        public string TableName { get; set; }
        
        [Required]
        public string SupplierColumns { get; set; }
        
        [Required]
        public string Categorisation { get; set; }
        
        [Required]
        public bool IsActive { get; set; }
        
        
    }
}
