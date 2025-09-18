using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Boost.Retail.Data.Models
{
    public class UserProductOverride
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int Id { get; set; }
        public string UserId { get; set; }
        public string GroupName { get; set; }
        public string MPN { get; set; }
        public string Colour { get; set; }
        public string Size { get; set; }
        public string SimpleColour { get; set; }
        public string SimpleSize { get; set; }
        public double RRP { get; set; }
        public double PromoRRP { get; set; }
        public int Stock { get; set; }
        public bool Permenant { get; set; }

        // Navigation property
        public int UserProductId { get; set; }
        [ForeignKey(nameof(UserProductId))]
        public UserProduct? UserProduct { get; set; }
    }
}
