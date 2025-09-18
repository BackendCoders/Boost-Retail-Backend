

using Boost.Retail.Domain.Enums;

namespace Boost.Retail.Data.DTO
{
    public class ProductDto
    {
        public string PartNumber { get; set; }
        public string MfrPartNumber { get; set; }
        public bool Major { get; set; }
        public Genders Gender { get; set; }
        public string Search1 { get; set; }
        public string Search2 { get; set; }
        public string Details { get; set; }
        public string Size { get; set; }
        public string Color { get; set; }
        public string Barcode { get; set; }
        public bool Current { get; set; }
        public string Year { get; set; }
        public int BoxQuantity { get; set; }
        public Seasons Season { get; set; }
        public decimal CostPrice { get; set; }
        public decimal SuggestedRRP { get; set; }
        public decimal StorePrice { get; set; }
        public decimal TradePrice { get; set; }
        public decimal MailOrderPrice { get; set; }
        public decimal WebPrice { get; set; }

        public bool Website { get; set; }
        public string ImageMain { get; set; }
        
        public decimal PromoPrice { get; set; }
        public DateTime? PromoStart { get; set; }
        public DateTime? PromoEnd { get; set; }
        public string PromoName { get; set; }

        public string Supplier1Code { get; set; }
        public string Make { get; set; }
        public string CatA { get; set; }
        public string CatB { get; set; }
        public string CatC { get; set; }
        public decimal BoxCost { get; set; }
    }
}
