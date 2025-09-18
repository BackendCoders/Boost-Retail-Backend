using Boost.Admin.Data;

namespace Boost.Admin.DTOs
{
    public class SearchDto
    {
        public DataSupplier Supplier { set; get; }
        public string Brand { set; get; } = string.Empty;
        public GenderOrAgeGroup Group { set; get; }
        public ProductType Type { set; get; }
        public string Year { set; get; } = string.Empty;
        public double MinCost { set; get; }
        public double MaxCost { set; get; }
        public double MinPrice { set; get; }
        public double MaxPrice { set; get; }
        public double MinSalePrice { set; get; }
        public double MaxSalePrice { set; get; }
        public int Skip { set; get; } = 0;
        public int Take { set; get; } = 100;
    }
}
