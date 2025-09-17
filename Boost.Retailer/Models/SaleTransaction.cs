using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Boost.Retail.Domain.Enums;

namespace Boost.Retail.Data.Models
{
    public class SaleTransaction: BaseEntity
    {

        [Required]
        public string TransactionReference { get; set; } = Guid.NewGuid().ToString();

        [Required]
        public string CustomerAcc { get; set; }

        [Required]
        public DateTime TransactionDate { get; set; } = DateTime.UtcNow;

        public TransactionStatus Status { get; set; } = TransactionStatus.Completed;

        public string Location { get; set; } 

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalAmount { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalDiscount { get; set; }

        public string DiscountCode { get; set; } = string.Empty;

        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalVAT { get; set; }

        public DateTime? PaymentDueDate { get; set; } // For pending payments

        public string TillId { get; set; } // To track which till processed the transaction

        public ICollection<SaleItem> SaleItems { get; set; } = new List<SaleItem>();

        public ICollection<ReturnTransaction> Returns { get; set; } = new List<ReturnTransaction>();

        public ICollection<SalePayment> PaymentTypes { get; set; } = new List<SalePayment>();

        public string InvoiceNumber { get; set; }
       
        public string OrderNo { get; set; }

        public string StaffCode { get; set; } = AppConstants.SYSTEM_STAFF_CODE;
       
        public decimal Profit { get; set; }

        public decimal Average { get; set; }
        public decimal Net { get; set; }
        public string Notes { get; set; } = string.Empty;
    }
}
