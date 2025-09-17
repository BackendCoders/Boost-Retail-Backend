using Boost.Retail.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace Boost.Retail.Data.Models
{
    public class SalePayment : BaseEntity
    {
        [Required]
        public int SaleTransactionId { get; set; }
        public PaymentType PaymentType { get; set; }
        public string Type { get; set; }
        public string TransactionRef { get; set; }
        public decimal Amount { get; set; }
    }
}
