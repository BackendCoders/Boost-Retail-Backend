namespace Boost.Admin.DTOs
{
    public class EposFeed
    {
        public string UserId { get; set; }
        public List<EposProduct> EposProducts { get; set; }

        // epos product
        public class EposProduct
        {
            public string PartNo { get; set; } = string.Empty;
            
            public string MPN { get; set; }
            public string? Barcode { get; set; }
            public string Title { get; set; }
            public string? GroupName { get; set; } = string.Empty;
            public string Brand { get; set; }
            public string Supplier { get; set; }
            public string Colour { get; set; }
            public string Size { get; set; }
            public string? Weight { get; set; } = string.Empty;
            public List<string>? Images { get; set; }
            public string? VideoUrl { get; set; }
            public string? SupplierDetailsUrl { get; set; }
            public string ShortDescription { get; set; }
            public string LongDescription { get; set; }
            public double Cost { get; set; }
            public double Price { get; set; }
            public double? PromoPrice { get; set; } = 0;
            public DateTime? PromoStart { get; set; }
            public DateTime? PromoEnd { get; set; }
            public double VatRate { get; set; }
            public string CategoryA { get; set; }
            public string CategoryB { get; set; }
            public string CategoryC { get; set; }
            public string GeometryJson { get; set; }
            public string SpecificationsJson { get; set; }
            public int? Year { get; set; }
        }
    }
}
