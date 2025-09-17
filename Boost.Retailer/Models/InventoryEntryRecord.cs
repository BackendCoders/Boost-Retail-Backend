using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using Boost.Retail.Domain.Enums;

namespace Boost.Retail.Data.Models
{
    public class InventoryEntryRecord : BaseEntity
    {
    
        [Required]
        [MinLength(2)]
        [DisplayName("Part Number")]
        [StringLength(5, ErrorMessage = "Part Number by can only be 5 characters.")]
        public string PartNumber { get; set; } = string.Empty;

        [Required]
        [MinLength(2)]
        [DisplayName("Location Code")]
        [StringLength(2, ErrorMessage = "Location Code by can only be 2 characters.")]
        public string LocationCode { get; set; } = string.Empty;

        [DisplayName("Stock Number")]
        [StringLength(10, ErrorMessage = "Stock Number by can only be 10 characters.")]
        [DefaultValue("")]
        public string StockNumber { get; set; } = string.Empty;

        [DisplayName("Serial Number")]
        [StringLength(10, ErrorMessage = "Stock Number by can only be 10 characters.")]
        [DefaultValue("")]
        public string SerialNumber { get; set; } = string.Empty;

        [DisplayName("Customer Account Number")]
        [StringLength(6, ErrorMessage = "Customer Account Number by can only be 6 characters.")]
        [DefaultValue("")]
        public string CustomerAccNo { get; set; } = string.Empty;

        [Required]
        [MinLength(2)]
        [DisplayName("Staff Code")]
        [StringLength(2, ErrorMessage = "Staff code by can only be 2 characters.")]
        public string StaffCode { get; set; } = string.Empty;

        [Required]
        [MinLength(6)]
        [DisplayName("Supplier Code")]
        [StringLength(6, ErrorMessage = "Supplier Code by can only be 6 characters.")]
        public string SupplierCode { get; set; } = string.Empty;

        [Required]
        [DefaultValue(0)]
        public int Quantity { get; set; }

        [Required]
        [DisplayName("Item Cost")]
        [DefaultValue(0)]

        public decimal Cost { get; set; } = 0;

        [DisplayName("Invoice Number")]
        [DefaultValue("")]
        [StringLength(15, ErrorMessage = "Invoice number by can only be 15 characters.")]
        public string InvoiceNumber { get; set; } = string.Empty;

        [DisplayName("Purchase Order Number")]
        [DefaultValue(AppConstants.DEFAULT_STOCKNO)]
        [StringLength(15, ErrorMessage = "Purchase order number by can only be 10 characters.")]
        public string PurchaseOrderNo { get; set; } = AppConstants.DEFAULT_STOCKNO;

        [Required]
        [DefaultValue(true)]
        public LabelPrintOption PrintLabelOption { get; set; }

        public StockItemStatus? StockItemStatus { get; set; }

        [DefaultValue(false)]
        public bool IsPrinted { get; set; } = false;
    }
}
