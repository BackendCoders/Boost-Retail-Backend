using Boost.Retail.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace Boost.Retail.Data.Models
{
    public class Layaway : BaseEntity
    {
        [MaxLength(5)]
        public string CustomerAccount { get; set; }

        [MaxLength(5)]
        public string PartNumber { get; set; }

        [MaxLength(20)]
        public string StockNumber { get; set; }

        [MaxLength(5)]
        public string LocationCode { get; set; }

        [MaxLength(5)]
        public string SalesCode { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal Discount { get; set; }
        public decimal VAT { get; set; }
        public string Notes { get; set; }

        [MaxLength(20)]
        public string WorkshopJobNo { get; set; }

        [MaxLength(20)]
        public string WebOrderNumber { get; set; }

        [MaxLength(20)]
        public string PurchaseOrderNumber { get; set; }

        public LayawayType LayawayType { get; set; }
    }
}
