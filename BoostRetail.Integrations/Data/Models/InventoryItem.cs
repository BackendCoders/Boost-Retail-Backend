
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BoostRetail.Integrations.SConnect.Data.Models
{
    public class InventoryItem
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string Search1 { get; set; }
        public string Search2 { get; set; }
        public string MfrPartNumber { get; set; }
        public string Make { get; set; }
        public string Barcode { get; set; }
        public string PartNumber { get; set; }
        public string CatA { get; set; }
        public string CatB { get; set; }
        public string CatC { get; set; }
        public string Year { get; set; }
        public decimal StorePrice { get; set; }
        public decimal CostPrice { get; set; }
        public bool Current { get; set; }
        public int VatCode { get; set; }
        public string Supplier1Code { get; set; }

        public DateTime CreatedOn { get; set; }
        public DateTime UpdatedOn { get; set; }

    }
}
