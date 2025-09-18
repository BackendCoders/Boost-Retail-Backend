namespace SIM.Suppliers.Raleigh
{
    public class FeedDto
    {
        public string ItemNumber { get; set; }
        public string Group { get; set; }
        public string ShortDesc { get; set; }
        public string EAN13 { get; set; }
        public string Brand { get; set; }
        public string Size { get; set; }
        public string BasicColour { get; set; }
        public string ModelYear { get; set; } // MODELYEAR | Model year
        public string LongDesc { get; set; }
        public string USP1 { get; set; }
        public string USP2 { get; set; }
        public string USP3 { get; set; }
        public string USP4 { get; set; }
        public string USP5 { get; set; }
        public string USP6 { get; set; }
        public string USP7 { get; set; }
        public string USP8 { get; set; }
        public string USP9 { get; set; } // MD-USP9 | USP-9
        public string USP10 { get; set; } // MD-USP10 | USP-10
        public string USP11 { get; set; } // MD-USP11 | USP-11
        public string USP12 { get; set; } // MD-USP12 | USP-12
        public string USP13 { get; set; } // MD-USP13 | USP-13
        public string ImageLink { get; set; }
        public string ImageLink1 { get; set; } // Image Link (1)
        public string ImageLink2 { get; set; } // Image Link (2)
        public decimal ConsumerPriceDefault { get; set; } // Consumer Price Default Price
        public string CountryOfOriginDescription { get; set; } // CNTRYORIG DESC | Country of Origin Description
        public string IntrastatCode { get; set; } // INTRASTAT | Intrastatcode
    }
}
