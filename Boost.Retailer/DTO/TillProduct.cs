namespace Boost.Retail.Data.DTO
{
    public class TillProduct
    {
        public string PartNumber { get; set; }
        public string Title { get; set; }
        public decimal CostPrice { get; set; }
        public decimal Price { get; set; }
        public bool IsPromo { get; set; }
        public decimal Discount { get; set; }
        public int StockHere { get; set; }
        public int StockTotal { get; set; }
    }

    public class TillProductShortcutDTO 
    {
        public string SetID { get; set; }
        public string PartNumber { get; set; }
        public string Title { get; set; }
        public string Category { get; set; }
        public string ImageURL { get; set; }
        public string BackgroundColor { get; set; }
        public string ForegroundColor { get; set; }
        public bool IsActive { get; set; }
    }
}
