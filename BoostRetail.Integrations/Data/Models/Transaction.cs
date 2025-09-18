using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BoostRetail.Integrations.SConnect.Data.Models
{
    public class Transaction
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public DateTime DateAndTime { get; set; }
        public string Location { get; set; }
        public string InvoiceNumber { get; set; }
        public int Count { get; set; }
        public string PartNumber { get; set; }
        public decimal Cost { get; set; }
        public decimal Sell { get; set; }
        public string InOut { get; set; }
        public decimal Vat { get; set; }
        public int Quantity { get; set; }
        public string Customer { get; set; }
    }
}
