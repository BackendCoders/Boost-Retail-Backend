using Boost.Retail.Domain;
using Boost.Retail.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace Boost.Retail.Data.Models
{
    
    public class Product : BaseEntity
    {
       

        public Product()
        {
            PartNumber = string.Empty;
            MfrPartNumber = string.Empty;
            MakeCode = string.Empty;
            Search1 = string.Empty;
            Search2 = string.Empty;
            Details = string.Empty;
            Size = string.Empty;
            Color = string.Empty;
            Barcode = string.Empty;
            Year = string.Empty;
            NominalCode = string.Empty;
            NominalSection = string.Empty;
            OfferCode = string.Empty;
            TillNote1 = string.Empty;
            TillNote2 = string.Empty;
            BinLocation1 = string.Empty;
            BinLocation2 = string.Empty;
            ImageMain = string.Empty;
            Image2 = string.Empty;
            Image3 = string.Empty;
            Image4 = string.Empty;
            Supplier1Code = string.Empty;
            Supplier2Code = string.Empty;
            CatACode = string.Empty;
            CatBCode = string.Empty;
            CatCCode = string.Empty;
            WebCat1Code = string.Empty;
            WebCat2Code = string.Empty;
            WebCat3Code = string.Empty;
            WebCat4Code = string.Empty;
            ShortDescription = string.Empty;
            FullDescription = string.Empty;
            WebsiteTitle = string.Empty;
            EbayTitle = string.Empty;
            GoogleShoppingTitle = string.Empty;
            Specifications = string.Empty;
            Geometry = string.Empty;
            Weight = 0;

            Make = string.Empty;
            CatA = string.Empty;
            CatB = string.Empty;
            CatC = string.Empty;
            MfrPartNumber2 = string.Empty;

            Major = false;
            Current = true;

            AllowDiscount = true;
            BoxQuantity = 1;
            CostPrice = 0;
            Discount = 0;
            Markup = 0;
            VatCode = 1;
            SuggestedRRP = 0;
            StorePrice = 0;
            TradePrice = 0;
            MailOrderPrice = 0;
            WebPrice = 0;
            KeyItem = false;
            AllowPoints = true;
            Website = true;
            WebOnly = false;

            PartsGarunteeMonths = 0;
            LabourGarunteeMonths = 0;
            MultibuyQuantity = 0;
            MultibuySave = 0;

            ItemId = string.Empty;
            Range = string.Empty;
            Finish = string.Empty;
            WebRef = 0;
            PromoName = string.Empty;


            ClickAndCollect = ClickCollect.InStoreAndHomeDelivery;
            LeadTime = LeadTimes.NextDay;
            Season = Seasons.All;
            Gender = Genders.Unisex;
            Suitability = AgeRange.Any;
            PrintLabel = PrintLabels.Yes;

            eBaySyncStock = true;
            eBayStockLevel = 0;

            
            TillNote3 = "";
            IsDiscontinued = false;

            WooID = string.Empty;
            WooParentID = string.Empty;
            IsWooVariation = false;
        }

        [Key]
        [MinLength(5)]
        public string PartNumber { get; set; }

        [MaxLength(50)]
        public string MfrPartNumber { get; set; }
        public bool Major { get; set; }
        public Genders Gender { get; set; }
        public AgeRange Suitability { get; set; }
        public DateTime? ExipryDate { get; set; }

        [MaxLength(20)]
        public string MakeCode { get; set; }

        [MaxLength(500)]
        public string Search1 { get; set; }

        [MaxLength(500)]
        public string Search2 { get; set; }

        public string Details { get; set; }

        [MaxLength(500)]
        public string Size { get; set; }

        [MaxLength(500)]
        public string Color { get; set; }

        [MaxLength(20)]
        public string Barcode { get; set; }

        public bool Current { get; set; }
        public PrintLabels PrintLabel { get; set; }
        public bool AllowDiscount { get; set; }
        public string Year { get; set; }
        public int BoxQuantity { get; set; }
        public string NominalSection { get; set; }
        public string NominalCode { get; set; }
        public Seasons Season { get; set; }
        public decimal CostPrice { get; set; }
        public decimal Discount { get; set; }
        public int DiscountPercentage { get; set; }
        public decimal Markup { get; set; }
        public int VatCode { get; set; }
        public decimal SuggestedRRP { get; set; }
        public decimal StorePrice { get; set; }
        public decimal TradePrice { get; set; }
        public decimal MailOrderPrice { get; set; }
        public decimal WebPrice { get; set; }
        public string OfferCode { get; set; }
        public string TillNote1 { get; set; } = string.Empty;
        public string TillNote2 { get; set; } = string.Empty;
        public string TillNote3 { get; set; } = string.Empty;
        public string TillNote4 { get; set; } = string.Empty;
        public string TillNote5 { get; set; } = string.Empty;
        public string TillNote6 { get; set; } = string.Empty;

        public bool Note1PrintPO { get; set; }
        public bool Note2PrintPO { get; set; }
        public bool Note3PrintPO { get; set; }
        public bool Note4PrintPO { get; set; }
        public bool Note5PrintPO { get; set; }
        public bool Note6PrintPO { get; set; }

        public bool Note1PrintInvoice { get; set; }
        public bool Note2PrintInvoice { get; set; }
        public bool Note3PrintInvoice { get; set; }
        public bool Note4PrintInvoice { get; set; }
        public bool Note5PrintInvoice { get; set; }
        public bool Note6PrintInvoice { get; set; }

        public bool KeyItem { get; set; }
        public bool AllowPoints { get; set; }
        public bool Website { get; set; }
        public bool WebOnly { get; set; }
        public string BinLocation1 { get; set; }
        public string BinLocation2 { get; set; }
        public int PartsGarunteeMonths { get; set; }
        public int LabourGarunteeMonths { get; set; }
        public decimal Weight { get; set; }
        public string ImageMain { get; set; }
        public string Image2 { get; set; }
        public string Image3 { get; set; }
        public string Image4 { get; set; }
        public decimal PromoPrice { get; set; }
        public DateTime? PromoStart { get; set; }
        public DateTime? PromoEnd { get; set; }
        public int MultibuyQuantity { get; set; }
        public decimal MultibuySave { get; set; }
        public DateTime? EstimatedArrivalDate { get; set; }
        public LeadTimes LeadTime { get; set; }
        public string Supplier1Code { get; set; }
        public string Supplier2Code { get; set; }

        public string CatACode { get; set; }
        public string CatBCode { get; set; }
        public string CatCCode { get; set; }

        public string WebCat1Code { get; set; }
        public string WebCat2Code { get; set; }
        public string WebCat3Code { get; set; }
        public string WebCat4Code { get; set; }

        public ClickCollect ClickAndCollect { get; set; }

        public string ShortDescription { get; set; }
        public string FullDescription { get; set; }
        public string WebsiteTitle { get; set; }
        public string EbayTitle { get; set; }
        public string GoogleShoppingTitle { get; set; }

        public string Specifications { get; set; }
        public string Geometry { get; set; }

        public string ItemId { get; set; }
        public string Range { get; set; }
        public string Finish { get; set; }
        public int WebRef { get; set; }

        public string Make { get; set; }
        public string CatA { get; set; }
        public string CatB { get; set; }
        public string CatC { get; set; }
        public string MfrPartNumber2 { get; set; }
        public string PromoName { get; set; }

        public DeliveryOptions DeliveryOption { get; set; }

        public bool eBaySyncStock { get; set; }
        public int eBayStockLevel { get; set; }

        public decimal BoxCost { get; set; }

        public bool IsDiscontinued { get; set; }
        public bool DoNotReOrder { get; set; }

        // Any once need true at a time. (either clearance or final clearance)
        public bool IsClearance { get; set; }
        public bool IsFinalClearance { get; set; }
        public bool IsExclusive { get; set; }

        public string WooID { get; set; }
        public string WooParentID { get; set; }

        public bool IsWooVariation { get; set; }

        
        public static Product Validate(Product item)
        {
            if (item.PartNumber == null) { item.PartNumber = ""; }
            if (item.MfrPartNumber == null) { item.MfrPartNumber = ""; }
            if (item.MakeCode == null) { item.MakeCode = ""; }
            if (item.Search1 == null) { item.Search1 = ""; }
            if (item.Search2 == null) { item.Search2 = ""; }
            if (item.Details == null) { item.Details = ""; }
            if (item.Size == null) { item.Size = ""; }
            if (item.Color == null) { item.Color = ""; }
            if (item.Barcode == null) { item.Barcode = ""; }
            if (item.Year == null) { item.Year = ""; }
            if (item.NominalCode == null) { item.NominalCode = ""; }
            if (item.NominalSection == null) { item.NominalSection = ""; }
            if (item.OfferCode == null) { item.OfferCode = ""; }
            if (item.TillNote1 == null) { item.TillNote1 = ""; }
            if (item.TillNote2 == null) { item.TillNote2 = ""; }
            if (item.BinLocation1 == null) { item.BinLocation1 = ""; }
            if (item.BinLocation2 == null) { item.BinLocation2 = ""; }
            if (item.ImageMain == null) { item.ImageMain = ""; }
            if (item.Image2 == null) { item.Image2 = ""; }
            if (item.Image3 == null) { item.Image3 = ""; }
            if (item.Image4 == null) { item.Image4 = ""; }
            if (item.Supplier1Code == null) { item.Supplier1Code = ""; }
            if (item.Supplier2Code == null) { item.Supplier2Code = ""; }
            if (item.CatACode == null) { item.CatACode = ""; }
            if (item.CatBCode == null) { item.CatBCode = ""; }
            if (item.CatCCode == null) { item.CatCCode = ""; }
            if (item.WebCat1Code == null) { item.WebCat1Code = ""; }
            if (item.WebCat2Code == null) { item.WebCat2Code = ""; }
            if (item.WebCat3Code == null) { item.WebCat3Code = ""; }
            if (item.WebCat4Code == null) { item.WebCat4Code = ""; }
            if (item.ShortDescription == null) { item.ShortDescription = ""; }
            if (item.FullDescription == null) { item.FullDescription = ""; }
            if (item.WebsiteTitle == null) { item.WebsiteTitle = ""; }
            if (item.EbayTitle == null) { item.EbayTitle = ""; }
            if (item.GoogleShoppingTitle == null) { item.GoogleShoppingTitle = ""; }
            if (item.Specifications == null) { item.Specifications = ""; }
            if (item.Geometry == null) { item.Geometry = ""; }
            if (item.ItemId == null) { item.ItemId = ""; }
            if (item.Range == null) { item.Range = ""; }
            if (item.Finish == null) { item.Finish = ""; }
            if (item.Make == null) { item.Make = ""; }
            if (item.CatA == null) { item.CatA = ""; }
            if (item.CatB == null) { item.CatB = ""; }
            if (item.CatC == null) { item.CatC = ""; }
            if (item.MfrPartNumber2 == null) { item.MfrPartNumber2 = ""; }

            return item;
        }

        /// <summary>
        /// Creates a string of the product data that is used to send to ABACUS API to create a part.
        /// </summary>
        /// <returns></returns>
        public string GetString()
        {
            var str = string.Empty;

            var header = @"PARTNO|MFRPARTNO|MFRPARTNO2|AI|MAKE|SEARCH1|SEARCH2|DETAILS|SIZE|COLOUR|YEAR|COST|VATCODE|STORERRP|TRADERRP|SRP|WEBRRP|MAILRRP" +
                "|SUPPLIER1|SUPPLIER2|CURRENT|WEBSITE|PRINTLABEL|BOXQTY|BIN1|BIN2|WEBONLY|KEYITEM|OFFERCODE|ETA|BARCODE|IMAGE|CATA|CATB|CATC|PROMORRP" +
                "|PROMOSTART|PROMOEND|SEASON|GENDER|WEBREF|RANGE|FINISH|PARTSGTEE|LABGTEE|DISCOUNT|MARKUP|ALLOWDISCOUNT|NOMCODE|NOMSEC|TILLNOTE1|TILLNOTE2" +
                "|ALLOWPOINTS|ITEMID|WEIGHT|MULTIBUYQTY|MULTIBUYSAVE|01 MIN|01 MAX|02 MIN|02 MAX|03 MIN|03 MAX|04 MIN|04 MAX|05 MIN|05 MAX|06 MIN|06 MAX|07 MIN|07 MAX|08 MIN|08 MAX|09 MIN|09 MAX|10 MIN|10 MAX|11 MIN|11 MAX|12 MIN|12 MAX|13 MIN|13 MAX|14 MIN|14 MAX|15 MIN|15 MAX|16 MIN|16 MAX|17 MIN|17 MAX|18 MIN|18 MAX|19 MIN|19 MAX|20 MIN|20 MAX|21 MIN|21 MAX|22 MIN|22 MAX|23 MIN|23 MAX|24 MIN|24 MAX|25 MIN|25 MAX|26 MIN|26 MAX|27 MIN|27 MAX|28 MIN|28 MAX|29 MIN|29 MAX|30 MIN|30 MAX|" + Environment.NewLine;

            var arr = new[]
            {
               PartNumber.PadRight(5), // 5
               MfrPartNumber.Truncate(20).PadRight(20), // 20
               MfrPartNumber2.Truncate(20).PadRight(20), // 20
               Major ? "A" : "I", // 1 
               MakeCode.ToUpper().Truncate(4).PadRight(4), //4
               Search1.ToUpper().Truncate(10).PadRight(10), //10
               Search2.ToUpper().Truncate(20).PadRight(20), //20
               Details.ToUpper().Truncate(30).PadRight(30), //30
               Size.ToUpper().Truncate(15).PadRight(15), // 15
               Color.ToUpper().Truncate(25).PadRight(25), //25
               Year.Length == 4 ?  Year.Substring(2).PadLeft(2) : Year.PadLeft(2), // 2
               CostPrice.ToString("N2").Replace(",","").PadLeft(6,'0'), // 6 
               VatCode.ToString().PadRight(1), // 1
               StorePrice.ToString("N2").Replace(",","").PadLeft(6,'0'), // 6
               TradePrice.ToString("N2").Replace(",","").PadLeft(6,'0'), //6
               SuggestedRRP.ToString("N2").Replace(",","").PadLeft(6,'0'), // 6
               WebPrice.ToString("N2").Replace(",","").PadLeft(6,'0'), //6
               MailOrderPrice.ToString("N2").Replace(",","").PadLeft(6,'0'), // 6
               Supplier1Code.Truncate(6).PadRight(6), // 6
               Supplier2Code.Truncate(6).PadRight(6), // 6
               Current ? "Y" : "N", // 1
               Website ? "Y" : "N", // 1
               PrintLabel == PrintLabels.Yes ? "Y" : PrintLabel == PrintLabels.No? "N" : "1", // 1
               BoxQuantity < 0 ? "1" : BoxQuantity.ToString().Truncate(3).PadRight(3), // 3
               BinLocation1.ToUpper().Truncate(5).PadRight(5), // 5
               BinLocation2.ToUpper().Truncate(5).PadRight(5), // 5
               WebOnly ? "Y" : "N", // 1
               KeyItem ? "Y" : "N", //1
               OfferCode.ToUpper().PadRight(1), // 1
               EstimatedArrivalDate?.ToString("ddMMyy") ?? "000000", // 6
               Barcode.Truncate(16).PadRight(16),
               string.IsNullOrEmpty(ImageMain) ? "".PadRight(30) : Path.GetFileNameWithoutExtension(ImageMain).Truncate(30).PadRight(30),
               CatACode.ToUpper().Truncate(4).PadRight(4), // 4
               CatBCode.ToUpper().Truncate(4).PadRight(4), // 4
               CatCCode.ToUpper().Truncate(4).PadRight(4), // 4
               PromoPrice.ToString("N2").Replace(",","").PadLeft(6,'0'), //6
               PromoStart?.ToString("ddMMyy").PadRight(6) ?? "000000", //6 DDMMYY
               PromoEnd?.ToString("ddMMyy").PadRight(6) ?? "000000", //6 DDMMYY
               Season == Seasons.All ? "A" : Season == Seasons.Summer ? "S" : "W",
               Gender == Genders.Female ? "F" : Gender == Genders.Male ? "M" : "U", // 1
               WebRef.ToString().ToUpper().Truncate(6).PadRight(6), // 6
               Range.ToUpper().PadRight(1), //1
               Finish.ToUpper().Truncate(8).PadRight(8), // 8
               PartsGarunteeMonths.ToString().ToUpper().PadLeft(2,'0'), // 2
               LabourGarunteeMonths.ToString().ToUpper().PadLeft(2,'0'), //2
               Discount.ToString("N2").Replace(",","").PadLeft(6,'0'), // 3
               Markup.ToString("N2").Replace(",","").PadLeft(6,'0'), // 3
               AllowDiscount? "Y" : "N", // 1
               NominalCode.ToUpper().Truncate(2).PadRight(2), // 2
               NominalSection.ToUpper().Truncate(4).PadRight(4), // 4
               TillNote1.ToUpper().Truncate(35).PadRight(35), // 35
               TillNote2.ToUpper().Truncate(35).PadRight(35), // 35
               AllowPoints ? "Y": "N", // 1
               ItemId.ToUpper().PadRight(1), //1
               Weight.ToString().Truncate(6).ToUpper().PadRight(6), // 6
               MultibuyQuantity.ToString().PadRight(3), // 3
               MultibuySave.ToString("N2").Replace(",","").PadLeft(6,'0'), // 6
            };

            str = $"{ string.Join("|", arr)}";

            return str;
        }

        /// <summary>
        /// Returns the stock level for the 2 digit location, this data is only available after GETSTOCKLEVEL has been called and the DLL_StockByLocation has been filled
        /// </summary>
        /// <param name="location"></param>
        /// <returns></returns>
        

        public bool IsUsingPromoPrice()
        {
            if (PromoPrice > 0)
            {
                // check start and end date
                if (PromoStart.HasValue)
                {
                    if (PromoStart.Value <= DateTime.Now)
                    {
                        if (PromoEnd.HasValue)
                        {
                            if (PromoEnd.Value >= DateTime.Now)
                            {
                                return true;
                            }
                            else // expired - use rrp
                            {
                                return false;
                            }
                        }
                        else
                            return true;  // no end date
                    }
                    else // future promo - use rrp
                    {
                        return false;
                    }
                }
                else // no promo start - use rrp
                {
                    return false;
                }
            }

            return false;
        }

    }
}
