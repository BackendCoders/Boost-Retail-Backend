using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using Boost.Retail.Domain.Enums;

namespace Boost.Retail.Data.Models
{
    public class PurchaseOrderHeader : BaseEntity
    {
        public PurchaseOrderHeader()
        {
            PartsOrdered = new List<PurchaseOrderItem>();
            IsImported = false;
        }

        [Key]
        [Required]
        [DisplayName("Order Number")]
        [MinLength(10)]
        [StringLength(10, ErrorMessage = "Purchase order number can not be longer than 10 digits.")]
        public string OrderNumber { get; set; } = string.Empty;

        //[Required(ErrorMessage = "There are no parts on this order, invalid.")]
        [DisplayName("Parts Ordered")]
        [NotMapped()]
        public virtual List<PurchaseOrderItem> PartsOrdered { get; set; }

        [Required]
        [MinLength(2)]
        [DisplayName("Raised by staff code")]
        [StringLength(25, ErrorMessage = "Raised by can only be 2 characters.")]
        public string RaisedByStaffCode { get; set; } = string.Empty;

        [Required]
        [DisplayName("Raised On Date")]
        [DataType(DataType.DateTime, ErrorMessage = "Raised on date is invalid.")]
        public DateTime RaisedOnDate { get; set; }

        [Required]
        [DisplayName("Carriage Cost")]
        [DefaultValue(0)]
        public double CarriageCost { get; set; } = 0;

        [DisplayName("Amended last by")]
        [StringLength(2, ErrorMessage = "Amended by can only be 2 characters.")]
        public string AmendedLastByCode { get; set; }

        [DisplayName("Amended On Date")]
        [DataType(DataType.DateTime, ErrorMessage = "Amended on date is invalid.")]
        public DateTime? AmendedLastOnDate { get; set; }

        [DisplayName("Closed by")]
        [StringLength(2, ErrorMessage = "Closed by can only be 2 characters.")]
        public string ClosedByCode { get; set; } = string.Empty;

        [DisplayName("Closed On Date")]
        [DataType(DataType.DateTime, ErrorMessage = "Closed on date is invalid.")]
        public DateTime? ClosedOnDate { get; set; }

        [DisplayName("Cancelled by")]
        [StringLength(2, ErrorMessage = "Cancelled by Name can only be 2 characters.")]
        public string CancelledByCode { get; set; }

        [DisplayName("Cancelled On Date")]
        [DataType(DataType.DateTime, ErrorMessage = "Cancelled on date is invalid.")]
        public DateTime? CancelledOnDate { get; set; }

        [Required]
        public PurchaseOrderStatus Status { get; set; }

        [Required]
        [DisplayName("Supplier Code")]
        [StringLength(6, ErrorMessage = "Supplier code can only be 6 characters.")]
        public string SupplierCode { get; set; } = string.Empty;

        /// <summary>
        /// Marks if this order was created by the original order system (cobol)
        /// </summary>
        [Required]
        public bool IsImported { get; set; }

        /// <summary>
        /// If (IsImported = true) this field will contain the json data required to reprint the report.
        /// </summary>
        public string JsonReport { get; set; }
        public bool DirectToStore { get; set; }

    }
}
