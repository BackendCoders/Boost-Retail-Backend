using CsvHelper.Configuration;
using SIM.Suppliers.Madison;
using SIM.Suppliers.Sportline;
using SIM.Suppliers.Trek;

namespace Boost.Admin.Configuration
{
    public class TrekFeedMap : ClassMap<TrekFeedDto>
    {
        public TrekFeedMap()
        {
            Map(m => m.Language).Name("LANGUAGE");
            Map(m => m.UPC).Name("UPC");
            Map(m => m.SKUId).Name("SKU_ID");
            Map(m => m.ParentId).Name("PARENT_ID");
            Map(m => m.Brand).Name("BRAND");
            Map(m => m.ShortDescription).Name("SHORT_DESCRIPTION");
            Map(m => m.LongDescription).Name("LONG_DESCRIPTION");
            Map(m => m.ParentName).Name("PARENT_NAME");
            Map(m => m.ModelYear).Name("MODEL_YEAR");
            Map(m => m.Size).Name("SIZE");
            Map(m => m.Gender).Name("GENDER");
            Map(m => m.Status).Name("STATUS");
            Map(m => m.ProductTypeLL).Name("PRODUCT_TYPE_LL");
            Map(m => m.Product).Name("PRODUCT");
            Map(m => m.Category).Name("CATEGORY");
            Map(m => m.WebshopItem).Name("WEBSHOP_ITEM");
            Map(m => m.Profile).Name("PROFILE");
            Map(m => m.P1Now).Name("P1_NOW");
            Map(m => m.CountryOfOriginAbbr).Name("COUNTRY_OF_ORIGIN_ABBR");
            Map(m => m.CountryOfOriginName).Name("COUNTRY_OF_ORIGIN_NAME");
            Map(m => m.Size1).Name("SIZE1");
            Map(m => m.RRPPrice).Name("RRPPRICE");
            Map(m => m.BasePrice).Name("BASEPRICE");
            Map(m => m.Currency).Name("CURRENCY");
            Map(m => m.SalePrice).Name("SALEPRICE");
            Map(m => m.SaleEFFDate).Name("SALE_EFFDATE");
            Map(m => m.SaleExpDate).Name("SALE_EXPDATE");
            Map(m => m.Standard).Name("STANDARD");
            Map(m => m.Bronze).Name("BRONZE");
            Map(m => m.IBD).Name("IBD");
            Map(m => m.Gold).Name("GOLD");
            Map(m => m.Premium).Name("PREMIUM");
            Map(m => m.Flagship).Name("FLAGSHIP");
            Map(m => m.ColorName).Name("COLOR_NAME");
            Map(m => m.SimpColrAbbr).Name("SIMPCOLR_ABBR");
            Map(m => m.SimpColr).Name("SIMPCOLR");
            Map(m => m.SimpColr1).Name("SIMPCLR_1");
            Map(m => m.SimpColr2).Name("SIMPCLR_2");
            Map(m => m.ColorCode).Name("COLOR_CODE");
            Map(m => m.ColorCode1).Name("COLOR_CODE_1");
            Map(m => m.ColorCode2).Name("COLOR_CODE_2");
            Map(m => m.ConsumerSiteURL).Name("CONSUMER_SITE_URL");
            Map(m => m.Image1URL).Name("IMAGE_1_URL");
            Map(m => m.Image2URL).Name("IMAGE_2_URL");
            Map(m => m.Image3URL).Name("IMAGE_3_URL");
            Map(m => m.ArchiveProductRedirect).Name("ARCHIVE_PRODUCT_REDIRECT");
            Map(m => m.BasicSpecFrame).Name("BASIC_SPEC_FRAME");
            Map(m => m.FrameMaterial).Name("FRAME_MATERIAL");
            Map(m => m.RimSize).Name("RIM_SIZE");
            Map(m => m.BrandFork).Name("BRAND_FORK");
            Map(m => m.BasicSpecFork).Name("BASIC_SPEC_FORK");
            Map(m => m.BrandRearsuspension).Name("BRAND_REARSUSPENSION");
            Map(m => m.BasicSpecSuspensionRear).Name("BASIC_SPEC_SUSPENSION__REAR");
            Map(m => m.BasicSpecWheels).Name("BASIC_SPEC_WHEELS");
            Map(m => m.BasicSpecTires).Name("BASIC_SPEC_TIRES");
            Map(m => m.BasicSpecHubFront).Name("BASIC_SPEC_HUB__FRONT");
            Map(m => m.BasicSpecHubRear).Name("BASIC_SPEC_HUB__REAR");
            Map(m => m.BasicSpecRims).Name("BASIC_SPEC_RIMS");
            Map(m => m.BrandShifter).Name("BRAND_SHIFTER");
            Map(m => m.BasicSpecShifters).Name("BASIC_SPEC_SHIFTERS");
            Map(m => m.BasicSpecDerailleurFront).Name("BASIC_SPEC_DERAILLEUR__FRONT");
            Map(m => m.BasicSpecDerailleurRear).Name("BASIC_SPEC_DERAILLEUR__REAR");
            Map(m => m.BasicSpecCrank).Name("BASIC_SPEC_CRANK");
            Map(m => m.BasicSpecBottomBracket).Name("BASIC_SPEC_BOTTOM_BRACKET");
            Map(m => m.BasicSpecCassette).Name("BASIC_SPEC_CASSETTE");
            Map(m => m.BasicSpecChain).Name("BASIC_SPEC_CHAIN");
            Map(m => m.BrandBrake).Name("BRAND_BRAKE");
            Map(m => m.BrakeType).Name("BRAKE_TYPE");
            Map(m => m.BasicSpecBrakeset).Name("BASIC_SPEC_BRAKESET");
            Map(m => m.BasicSpecRotorSize).Name("BASIC_SPEC_ROTOR_SIZE");
            Map(m => m.BasicSpecPedals).Name("BASIC_SPEC_PEDALS");
            Map(m => m.BasicSpecSaddle).Name("BASIC_SPEC_SADDLE");
            Map(m => m.BasicSpecSeatpost).Name("BASIC_SPEC_SEATPOST");
            Map(m => m.BasicSpecHandlebar).Name("BASIC_SPEC_HANDLEBAR");
            Map(m => m.BasicSpecStem).Name("BASIC_SPEC_STEM");
            Map(m => m.BasicSpecHeadset).Name("BASIC_SPEC_HEADSET");
            Map(m => m.BasicSpecGrips).Name("BASIC_SPEC_GRIPS");
            Map(m => m.BasicSpecChainguardCase).Name("BASIC_SPEC_CHAINGUARD_CASE");
            Map(m => m.BasicSpecFenders).Name("BASIC_SPEC_FENDERS");
            Map(m => m.BasicSpecCarrierFront).Name("BASIC_SPEC_CARRIER__FRONT");
            Map(m => m.BasicSpecCarrierRear).Name("BASIC_SPEC_CARRIER__REAR");
            Map(m => m.BasicSpecLightFront).Name("BASIC_SPEC_LIGHT__FRONT");
            Map(m => m.BasicSpecLightRear).Name("BASIC_SPEC_LIGHT__REAR");
            Map(m => m.BasicSpecDynamo).Name("BASIC_SPEC_DYNAMO");
            Map(m => m.BasicSpecKickstand).Name("BASIC_SPEC_KICKSTAND");
            Map(m => m.BasicSpecLock).Name("BASIC_SPEC_LOCK");
            Map(m => m.BasicSpecExtras).Name("BASIC_SPEC_EXTRAS");
            Map(m => m.BasicSpecBattery).Name("BASIC_SPEC_BATTERY");
            Map(m => m.BasicSpecController).Name("BASIC_SPEC_CONTROLLER");
            Map(m => m.BasicSpecMotor).Name("BASIC_SPEC_MOTOR");
            Map(m => m.AbbreviatedSpecMotorPower).Name("ABBREVIATED_SPEC_MOTOR_POWER");
            Map(m => m.ImageAlt1URL).Name("IMAGE_ALT1_URL");
            Map(m => m.ImageAlt2URL).Name("IMAGE_ALT2_URL");
            Map(m => m.ImageAlt3URL).Name("IMAGE_ALT3_URL");
            Map(m => m.ImageAlt4URL).Name("IMAGE_ALT4_URL");
            Map(m => m.ImageAlt5URL).Name("IMAGE_ALT5_URL");
            Map(m => m.ImageAlt6URL).Name("IMAGE_ALT6_URL");
            Map(m => m.ImageAlt7URL).Name("IMAGE_ALT7_URL");
            Map(m => m.ImageAlt8URL).Name("IMAGE_ALT8_URL");
            Map(m => m.ImageAlt9URL).Name("IMAGE_ALT9_URL");
            Map(m => m.Overview).Name("OVERVIEW");
            Map(m => m.KeyFeatures).Name("KEY_FEATURES");
            Map(m => m.TechFeatures).Name("TECH_FEATURES");
            Map(m => m.SizeChartRiderHeight).Name("SIZE_CHART_RIDER_HEIGHT");
            Map(m => m.SizeChartRiderInseam).Name("SIZE_CHART_RIDER_INSEAM");
        }
    }

    public class MadisonFeedMap : ClassMap<MadisonFeedDto>
    {
        public MadisonFeedMap()
        {
            Map(m => m.Product).Index(0);
            Map(m => m.Description).Index(1);
            Map(m => m.Barcode).Index(2);
            Map(m => m.Colour).Index(3);
            Map(m => m.Size).Index(4);
            Map(m => m.Brand).Index(5);
            Map(m => m.Hierachy).Index(6);
            Map(m => m.UOM).Index(7);
            Map(m => m.SRP).Index(8);
            Map(m => m.TradePrice).Index(9);
            Map(m => m.VatCode).Index(10);
            Map(m => m.VatRate).Index(11);
            Map(m => m.WebFriendlyDescription).Index(12);
            Map(m => m.ImageFilename).Index(13);
            Map(m => m.StockFlag).Index(14);
            Map(m => m.PackContents).Index(15);
            Map(m => m.DivisionCode).Index(16);
            Map(m => m.DiscontinuedFlag).Index(17);
            Map(m => m.SupersessionCode).Index(18);
            Map(m => m.UnitPrice).Index(19);
            Map(m => m.CountryOfOrigin).Index(20);
            Map(m => m.BreakQty1).Index(21);
            Map(m => m.BreakPrice1).Index(22);
            Map(m => m.BreakQty2).Index(21);
            Map(m => m.BreakPrice2).Index(22);
            Map(m => m.BreakQty3).Index(21);
            Map(m => m.BreakPrice3).Index(22);
            Map(m => m.BreakQty4).Index(21);
            Map(m => m.BreakPrice4).Index(22);
            Map(m => m.BreakQty5).Index(21);
            Map(m => m.BreakPrice5).Index(22);
        }
    }

    public class SportlineFeedMap : ClassMap<SportlineFeedDto>
    {
        public SportlineFeedMap()
        {
            Map(m => m.ProductCode).Index(0);
            Map(m => m.ShortDescription).Index(1);
            Map(m => m.Description).Index(2);
            Map(m => m.LongWebText).Index(3);
            Map(m => m.Barcode).Index(4);
            Map(m => m.VatRate).Index(5);
            Map(m => m.UnitOfMeasure).Index(6);
            Map(m => m.RRP).Index(7);
            Map(m => m.LeadTime).Index(8);
            Map(m => m.Weight).Index(9);
            Map(m => m.ImageName).Index(10);
            Map(m => m.AlternativeImage1).Index(11);
            Map(m => m.AlternativeImage2).Index(12);
            Map(m => m.AlternativeImage3).Index(13);
            Map(m => m.SupplierCode).Index(14);
            Map(m => m.Size).Index(15);
            Map(m => m.BasicColour).Index(16);
            Map(m => m.BrandColour).Index(17);
            
            Map(m => m.Brand).Index(18);
            Map(m => m.VariantGrouping).Index(19);
            Map(m => m.OuterQuantity).Index(20);
            Map(m => m.TradePrice).Index(21);
            Map(m => m.YourPrice).Index(22);
            Map(m => m.StockistPrice).Index(23);
            Map(m => m.KeyPrice).Index(24);
            Map(m => m.SuperPrice).Index(25);
            Map(m => m.PremierPrice).Index(26);
            Map(m => m.Category).Index(27);
            Map(m => m.SubCategory).Index(28);
            Map(m => m.SubSubCategory).Index(29);
            Map(m => m.Keywords).Index(30);
            Map(m => m.Replacement).Index(31);
            Map(m => m.ExpectedDate).Index(32);
            Map(m => m.Specification).Index(33);
            Map(m => m.CommodityCode).Index(34);
        }
    }
}
