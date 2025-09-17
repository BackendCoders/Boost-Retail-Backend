using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BoostRetail.Integrations.SConnect.Data.Models
{
    public class Location
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int BranchId { get; set; }
        public string BranchName { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Address3 { get; set; }
        public string Address4 { get; set; }
        public string Postcode { get; set; }
        public string SpecializedLocatorId { get; set; }
        public string GeneralEmailAddress { get; set; }
        public string MainTelephone { get; set; }

    }
}
