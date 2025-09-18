using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using Boost.Retail.Domain.Enums;

namespace Boost.Retail.Data.Models
{
    public class Supplier : BaseEntity
    {
        [Required]
        [DisplayName("Account No")]
        [MinLength(6)]
        [StringLength(6, ErrorMessage = "Account No can only be 6 characters.")]
        public string AccountNo { set; get; } = string.Empty;

        [Required]
        [DisplayName("Supplier Name")]
        [MinLength(1)]
        [StringLength(30, ErrorMessage = "Name can only be 30 characters.")]
        public string Name { set; get; } = string.Empty;

        [DisplayName("Address Line 1")]
        [StringLength(30, ErrorMessage = "Address Line 1 can only be 30 characters.")]
        [DefaultValue("")]
        public string Address1 { set; get; } = string.Empty;

        [DisplayName("Address Line 2")]
        [StringLength(30, ErrorMessage = "Address Line 2 can only be 30 characters.")]
        [DefaultValue("")]
        public string Address2 { set; get; } = string.Empty;

        [DisplayName("Address Line 3")]
        [StringLength(30, ErrorMessage = "Address Line 3 can only be 30 characters.")]
        [DefaultValue("")]
        public string Address3 { set; get; } = string.Empty;

        [DisplayName("Address Line 4")]
        [StringLength(30, ErrorMessage = "Address Line 4 can only be 30 characters.")]
        [DefaultValue("")]
        public string Address4 { set; get; } = string.Empty;

        [DisplayName("Postcode")]
        [StringLength(8, ErrorMessage = "Postcode can only be 8 characters.")]
        [DefaultValue("")]
        public string Postcode { set; get; } = string.Empty;

        [DisplayName("Telephone")]
        [StringLength(15, ErrorMessage = "Telephone can only be 15 characters.")]
        [DefaultValue("")]
        public string Telephone { set; get; } = string.Empty;

        [DisplayName("Fax")]
        [StringLength(15, ErrorMessage = "Fax can only be 15 characters.")]
        [DefaultValue("")]
        public string Fax { set; get; } = string.Empty;

        [DisplayName("Email")]
        [DefaultValue("")]
        [StringLength(50, ErrorMessage = "Email can only be 50 characters.")]
        [EmailAddress(ErrorMessage = "Invalid email address format.")]
        public string Email { set; get; } = string.Empty;

        [DisplayName("B2B File Name")]
        [StringLength(10, ErrorMessage = "B2B ID can only be 10 characters.")]
        [DefaultValue("")]
        public string B2BFileName { set; get; } = string.Empty;

        [DisplayName("B2B File Type")]
        [DefaultValue(B2bFileType.CSV)]
        public B2bFileType B2BFileType { set; get; } = B2bFileType.CSV;

        [DisplayName("B2B File - Header Row")]
        [DefaultValue(false)]
        public bool B2BFileHasHeaderRow { set; get; } = false;

        [DisplayName("B2B File - Append Location")]
        [DefaultValue(false)]
        public bool B2BFileAppendLocationCode { set; get; } = false;

        [Required]
        [DisplayName("Settlement Discount")]
        [DefaultValue(0)]

        public decimal SettlementDiscount { set; get; } = 0;

        [Required]
        [DisplayName("Settlement Discount")]
        [DefaultValue(0)]

        public decimal CarriagePaidAmount { get; set; } = 0;

        [DefaultValue(false)]
        public bool IsDeleted { get; set; } = false;
    }
}
