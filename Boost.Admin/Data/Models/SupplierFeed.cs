using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Boost.Admin.Data.Models
{
    public class SupplierFeed
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public int SupplierId { get; set; }

        [Required]
        public int FeedNameId { get; set; }

        [Required]
        public string FeedAddress { get; set; } = string.Empty;

        [Required]
        public string APIKey { get; set; } = string.Empty;

        [Required]
        public string UserName { get; set; } = string.Empty;
        
        [Required]
        public string Password { get; set; } = string.Empty;

        [Required]
        public bool IsActive { get; set; }

    }
}
