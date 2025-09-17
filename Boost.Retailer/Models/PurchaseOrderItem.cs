using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using Boost.Retail.Domain.Enums;

namespace Boost.Retail.Data.Models
{
    public class PurchaseOrderItem : BaseEntity
    {
        public PurchaseOrderItem()
        {
            CreatedOnDate = DateTime.Now;

            OrderNumber = AppConstants.UNRAISED_ORDERNO; // 10
            QtyRecieved = 0;
            CustomerAccNo = string.Empty;
            XmasClub = false;
            SmsCustomerOnArrival = true;
            Notes = string.Empty;
            Reason = PurchaseOrderReason.UNKNOWN_IMPORTED;
            BoxQty = 1;
        }

        [Required]
        [DisplayName("Purchase Order Number")]
        [MinLength(10)]
        [StringLength(10, ErrorMessage = "Purchase order number can not be longer than 10 digits.")]
        public string OrderNumber { get; set; }

        [Required]
        [DisplayName("Sequence Id")]
        [Range(1, 1000, ErrorMessage = "Sequence Id invalid (1-100000).")]
        public int SequenceId { get; set; }

        [Required]
        [MinLength(5, ErrorMessage = "The part number must be 5 characters.")]
        [StringLength(5, ErrorMessage = "The part number is to long.")]
        [DisplayName("Part Number")]
        public string PartNumber { get; set; } = string.Empty;

        [Required]
        [MinLength(2, ErrorMessage = "The Mfrpart number must be 2 characters or more.")]
        [StringLength(25, ErrorMessage = "The Mfrpart number is to long.")]
        [DisplayName("Mfr Part Number")]
        public string MfrPartNumber { get; set; } = string.Empty;


        [DefaultValue("")]
        [StringLength(150, ErrorMessage = "The Description is to long.")]
        [DisplayName("Description")]
        public string Description { get; set; } = string.Empty;

        [Required]
        [DisplayName("Box Quantity")]
        [DefaultValue(1)]
        [Range(1, 100000, ErrorMessage = "Box Qty invalid (1-100000).")]
        public int BoxQty { get; set; }

        [Required]
        [DisplayName("Quantity Required")]
        [Range(1, 100000, ErrorMessage = "Qty Required invalid (1-100000).")]
        public int QtyRequired { get; set; }

        [Required]
        [DisplayName("Cost Price")]
        [Range(0, 10000000, ErrorMessage = "Cost Price Invalid (0-10000000).")]

        public decimal CostPrice { get; set; }

        [Required]
        [DisplayName("Quantity Recieved")]
        [DefaultValue(0)]
        [Range(0, 100000, ErrorMessage = "Qty recieved invalid (1-100000).")]
        public int QtyRecieved { get; set; }

        [Required]
        [DisplayName("Stock Location Code")]
        [MinLength(2)]
        [StringLength(2, ErrorMessage = "Order for location code can only be 2 characters.")]
        public string StockLocationCode { get; set; } = string.Empty;

        [Required]
        [DisplayName("Deliver To Location Code")]
        [MinLength(2)]
        [StringLength(2, ErrorMessage = "Deliver to location code can only be 2 characters.")]
        public string DeliveryLocationCode { get; set; } = string.Empty;

        [DefaultValue("")]
        [DisplayName("Customer Account Number")]
        [StringLength(6, ErrorMessage = "Customer account number can only be 6 characters.")]
        public string CustomerAccNo { get; set; }

        [Required]
        [DisplayName("Major Item")]
        public bool IsMajor { get; set; }

        [Required]
        [DefaultValue(false)]
        [DisplayName("Xmas Club")]
        public bool XmasClub { get; set; }

        [Required]
        [DefaultValue(true)]
        [DisplayName("Sms Customer on Arrival")]
        public bool SmsCustomerOnArrival { get; set; }

        [DefaultValue("")]
        [DisplayName("Notes")]
        [StringLength(500, ErrorMessage = "Notes can only be 500 characters.")]
        public string Notes { get; set; } = string.Empty;

        [Required]
        [MinLength(2)]
        [DisplayName("Ordered by")]
        [StringLength(2, ErrorMessage = "Ordered code by can only be 2 characters.")]
        public string OrderedByCode { get; set; } = string.Empty;

        [Required]
        [MinLength(6)]
        [DisplayName("Supplier Code")]
        [StringLength(6, ErrorMessage = "Supplier code can only be 6 characters.")]
        public string SupplierCode { get; set; } = string.Empty;

        [Required]
        [DisplayName("Created On Date")]
        [DataType(DataType.DateTime, ErrorMessage = "Created on date is invalid.")]
        public DateTime CreatedOnDate { get; set; }

        [Required]
        [DisplayName("Reason")]
        public PurchaseOrderReason Reason { get; set; }

        public int InternalOrderRefID
        {
            set; get;
        }


        // ignore
        [NotMapped()]
        public bool ItemsOutstanding { get { return QtyRequired - QtyRecieved != 0; } }

        [NotMapped()]
        public bool DirectToStore { set; get; }

    }
}
