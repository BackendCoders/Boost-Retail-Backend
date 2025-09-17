using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Boost.Retail.Data.Models
{
    public class ReturnTransaction : BaseEntity
    {

        [Required]
        public int SaleTransactionId { get; set; }

        [Required]
        public int SaleItemId { get; set; }

        [Required]
        public int ReturnQuantity { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal RefundAmount { get; set; }

        public string Reason { get; set; }

        [Required]
        public DateTime ReturnDate { get; set; } = DateTime.UtcNow;

        public string TillId { get; set; }
    }
}
