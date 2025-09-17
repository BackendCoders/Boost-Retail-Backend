using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace Boost.Retail.Data.Models
{
    public class Location : BaseEntity
    {
        [Key]
        [Required]
        [DisplayName("Location Code")]
        [MinLength(2)]
        [StringLength(2, ErrorMessage = "Location Code can only be 4 characters.")]
        public string Code { get; set; } = string.Empty;

        [Required]
        [DisplayName("Location Name")]
        [MinLength(1)]
        [StringLength(30, ErrorMessage = "Name can only be 30 characters.")]
        public string Name { get; set; } = string.Empty;

        [DisplayName("CompanyNumber")]
        [MinLength(0)]
        [StringLength(30, ErrorMessage = "Company Number can only be 30 characters.")]
        [DefaultValue("")]
        public string CompanyNumber { get; set; } = string.Empty;

        [DisplayName("Address1")]
        [MinLength(0)]
        [DefaultValue("")]
        public string Address1 { get; set; } = string.Empty;

        [DisplayName("Address2")]
        [MinLength(0)]
        [DefaultValue("")]
        public string Address2 { get; set; } = string.Empty;

        [DisplayName("Address3")]
        [MinLength(0)]
        [DefaultValue("")]
        public string Address3 { get; set; } = string.Empty;

        [DisplayName("Address4")]
        [MinLength(0)]
        [DefaultValue("")]
        public string Address4 { get; set; } = string.Empty;

        [DisplayName("Postcode")]
        [MinLength(0)]
        [DefaultValue("")]
        public string Postcode { get; set; } = string.Empty;

        [DisplayName("GeneralEmailAddress")]
        [MinLength(0)]
        [DefaultValue("")]
        public string GeneralEmailAddress { get; set; } = string.Empty;

        [DisplayName("MainTelephone")]
        [MinLength(0)]
        [DefaultValue("")]
        public string MainTelephone { get; set; } = string.Empty;

        [DefaultValue(false)]
        public bool IsDeleted { get; set; } = false;


        [DefaultValue("")]
        [StringLength(200, ErrorMessage = "Store Website URL  can only be 200 characters.")]
        public string StoreWebsiteURL { get; set; } = string.Empty;

        [DefaultValue("")]
        [StringLength(200, ErrorMessage = "Admin Name  can only be 200 characters.")]
        public string AdminName { get; set; } = string.Empty;

        [DefaultValue("")]
        [StringLength(200, ErrorMessage = "Admin Email can only be 200 characters.")]
        public string AdminEmail { get; set; } = string.Empty;

        [DefaultValue("")]
        [StringLength(200, ErrorMessage = "Account Name can only be 200 characters.")]
        public string AccountName { get; set; } = string.Empty;

        [DefaultValue("")]
        [StringLength(200, ErrorMessage = "Account Email can only be 200 characters.")]
        public string AccountEmail { get; set; } = string.Empty;

        [DefaultValue(false)]
        public bool KeyLocation { get; set; } = false;

        [DefaultValue(false)]
        public bool IsActive { get; set; } = false;
        

        [NotMapped]
        public string DisplayText { get { return $"({Code}) {Name}"; } }

        public override string ToString() { return Name; }
    }
}
