
using Boost.Retail.Data.Models;
using System.ComponentModel.DataAnnotations;

namespace Boost.Retail.Data.DTO
{
    public class SaleRequest
    {
        [Required]
        public string CustomerAccount { get; set; }
        public List<SaleItemRequest> Items { get; set; }
        public List<SalePayment> PaymentTypes { get; set; }
        public DateTime? PaymentDueDate { get; set; }
        public string TillId { get; set; }

        [Required]
        public string Location { get; set; }

        [Required]
        public string SalesCode { get; set; }

        public string DiscountCode { get; set; } = string.Empty;
        public string Notes { get; set; } = string.Empty;
        public string InvoiceNumber { get; set; } = string.Empty;
        public string OrderNo { get; set; } = string.Empty;
    }

    public class SaleItemRequest
    {
        [Required]
        public string PartNumber { get; set; }

        [Required]
        public int Quantity { get; set; }

        [Required]
        public decimal UnitPrice { get; set; }
        public decimal Discount { get; set; }
        public decimal VAT { get; set; }

        [Required]
        public decimal CostPrice { get; set; }
        public bool IsPromo { get; set; }

        public string StockNumber { get; set; }
    }

    public class ReturnRequest
    {
        [Required]
        public int SaleTransactionId { get; set; }
        public int SaleItemId { get; set; }
        public int ReturnQuantity { get; set; }
        public string Reason { get; set; }
        public string TillId { get; set; }

        [Required]
        public string Location { get; set; }
    }

    public class CompletePaymentRequest
    {
        public string SaleTransactionRef { get; set; }
        public List<SalePayment> PaymentTypes { get; set; }
        public string TillId { get; set; }
    }

    public class StockCheckRequest
    {
        public string PartNumber { get; set; }
        public int Quantity { get; set; }
        public string Location { get; set; }
    }
}
