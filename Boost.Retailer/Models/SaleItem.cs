using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Boost.Retail.Data.Models
{
    public class SaleItem : BaseEntity
    {
   
        [Required]
        public int SaleTransactionId { get; set; }

        [Required]
        public string PartNumber { get; set; }

        [Required]
        public int Quantity { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal CostPrice { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal UnitPrice { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Discount { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal VAT { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalPrice => (UnitPrice * Quantity) - Discount + VAT;

        public string StockNumber {  get; set; }
        public bool IsPromo { get; set; }
    }

}
