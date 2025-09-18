
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Boost.Retail.Data.Models
{
    public class UserProductGroupOverride
    {
        public UserProductGroupOverride()
        {
            PromoStart = DateTime.MinValue;
            PromoEnd = DateTime.MinValue;
        }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string UserId { get; set; }
        
        [Key]
        public string GroupName { get; set; }

        public string Title { get; set; }
        public string Brand { get; set; }
        public int Year { get; set; }
        public DateTime? PromoStart { get; set; } = DateTime.MinValue;
        public DateTime? PromoEnd { get; set; } = DateTime.MinValue;

        public string Category1 { get; set; }
        public string Category2 { get; set; }
        public string Category3 { get; set; }
        public string Model { get; set; }

        public string ShortDescription { get; set; }
        public string FullDescription { get; set; }
        public string Specification { get; set; }
        public string Geometry { get; set; }
        public bool Permenant { get; set; }
    }
}
