namespace Boost.Retail.Data.DTO
{
    public class SetStockRequest
    {
        public string PartNumber { get; set; }
        public string LocationCode { get; set; } 
        public int Stock { get; set; }

        public List<string> StockNumbers { get; set; } = new List<string>();

    }
}
