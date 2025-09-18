using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace SIM.Suppliers.Giant
{
    #region Giant API
    public partial class GiantApiDto
    {
        [JsonProperty("Products")]
        public List<Product> Products { get; set; }
    }

    public partial class Product
    {
        [JsonProperty("Id")]
        public long Id { get; set; }

        [JsonProperty("GlobalId")]
        public long GlobalId { get; set; }

        [JsonProperty("CountryIsoCode")]
        public string CountryIsoCode { get; set; }

        [JsonProperty("Brand")]
        public long Brand { get; set; }

        [JsonProperty("Name")]
        public string Name { get; set; }

        [JsonProperty("Type")]
        public long Type { get; set; }

        [JsonProperty("Features")]
        public dynamic[] Features { get; set; }

        [JsonProperty("MetaTitle")]
        public string MetaTitle { get; set; }

        [JsonProperty("MetaDescription")]
        public string MetaDescription { get; set; }

        [JsonProperty("Images")]
        public Image[] Images { get; set; }

        [JsonProperty("Skus")]
        public Skus[] Skus { get; set; }

        [JsonProperty("ProductAttributes")]
        public ProductAttribute[] ProductAttributes { get; set; }

        [JsonProperty("CollectionName")]
        [Newtonsoft.Json.JsonConverter(typeof(ParseStringConverter))]
        public long CollectionName { get; set; }

        [JsonProperty("BikeSeriesName")]
        public string BikeSeriesName { get; set; }

        [JsonProperty("BikeSeriesDescription")]
        public string BikeSeriesDescription { get; set; }

        [JsonProperty("Tagline")]
        public string Tagline { get; set; }

        [JsonProperty("Filters")]
        public Filter[] Filters { get; set; }

        [JsonProperty("KeyPerformanceFactors")]
        public KeyPerformanceFactor[] KeyPerformanceFactors { get; set; }

        [JsonProperty("Categories")]
        public string[] Categories { get; set; }

        [JsonProperty("Technologies")]
        public Technology[] Technologies { get; set; }

        [JsonProperty("PowerReviewId")]
        public string PowerReviewId { get; set; }

        [JsonProperty("Colors")]
        public string[] Colors { get; set; }

        [JsonProperty("NavigationIds")]
        public string[] NavigationIds { get; set; }

        [JsonProperty("CurrencyCode")]
        public string CurrencyCode { get; set; }

        [JsonProperty("RetailerProductLabels")]
        public dynamic[] RetailerProductLabels { get; set; }

        [JsonProperty("HasBikeFitCategory")]
        public bool HasBikeFitCategory { get; set; }

        [JsonProperty("DefaultPedalId")]
        public long DefaultPedalId { get; set; }

        [JsonProperty("AllowanceLevel")]
        public long AllowanceLevel { get; set; }

        [JsonPropertyName("Specifications")]
        public string Specifications { get; set; }

        [JsonPropertyName("SpecificationsObject")]
        public SpecificationsObject SpecificationsObject { get; set; }

    }

    public partial class Filter
    {
        [JsonProperty("FilterId")]
        public long FilterId { get; set; }

        [JsonProperty("FilterName")]
        public string FilterName { get; set; }

        [JsonProperty("LocalizedFilterNames")]
        public dynamic[] LocalizedFilterNames { get; set; }

        [JsonProperty("FilterNameLocalized")]
        public string FilterNameLocalized { get; set; }

        [JsonProperty("FilterValue")]
        public string FilterValue { get; set; }

        [JsonProperty("LocalizedFilterValues")]
        public dynamic[] LocalizedFilterValues { get; set; }

        [JsonProperty("FilterValueLocalized")]
        public string FilterValueLocalized { get; set; }
    }

    public partial class Image
    {
        [JsonProperty("Path")]
        public Uri Path { get; set; }

        [JsonProperty("SkuId", NullValueHandling = NullValueHandling.Ignore)]
        public string SkuId { get; set; }

        [JsonProperty("CloudinaryPublicId")]
        public string CloudinaryPublicId { get; set; }
    }

    public partial class KeyPerformanceFactor
    {
        [JsonProperty("Title")]
        public string Title { get; set; }

        [JsonProperty("Text")]
        public string Text { get; set; }

        [JsonProperty("Sortorder")]
        public long Sortorder { get; set; }
    }

    public partial class ProductAttribute
    {
        [JsonProperty("Key")]
        public string Key { get; set; }

        [JsonProperty("Label")]
        public string Label { get; set; }

        [JsonProperty("Value")]
        public string Value { get; set; }
    }

    public partial class Skus
    {
        [JsonProperty("Id")]
        public string Id { get; set; }

        [JsonProperty("Color")]
        public string Color { get; set; }

        [JsonProperty("Size")]
        public string Size { get; set; }

        [JsonProperty("Usage")]
        public string Usage { get; set; }

        [JsonProperty("Discontinued")]
        public bool Discontinued { get; set; }

        [JsonProperty("Frame")]
        public Frame Frame { get; set; }

        [JsonProperty("ShowInShop")]
        public bool ShowInShop { get; set; }

        [JsonProperty("Barcode")]
        public string Barcode { get; set; }

        [JsonProperty("Images")]
        public Image[] Images { get; set; }
    }

    public partial class Frame
    {
        [JsonProperty("SizeName")]
        public string SizeName { get; set; }

        [JsonProperty("SeatTubeLength")]
        public long SeatTubeLength { get; set; }

        [JsonProperty("SeatTubeAngle")]
        public double SeatTubeAngle { get; set; }

        [JsonProperty("SeatTubeAngleMid")]
        public double SeatTubeAngleMid { get; set; }

        [JsonProperty("SeatTubeAngleHigh")]
        public double SeatTubeAngleHigh { get; set; }

        [JsonProperty("TopTubeLength")]
        public double TopTubeLength { get; set; }

        [JsonProperty("TopTubeLengthMid")]
        public double TopTubeLengthMid { get; set; }

        [JsonProperty("TopTubeLengthHigh")]
        public double TopTubeLengthHigh { get; set; }

        [JsonProperty("HeadTubeLength")]
        public long HeadTubeLength { get; set; }

        [JsonProperty("HeadTubeAngle")]
        public double HeadTubeAngle { get; set; }

        [JsonProperty("HeadTubeAngleMid")]
        public double HeadTubeAngleMid { get; set; }

        [JsonProperty("HeadTubeAngleHigh")]
        public double HeadTubeAngleHigh { get; set; }

        [JsonProperty("ForkRake")]
        public long ForkRake { get; set; }

        [JsonProperty("Trail")]
        public double Trail { get; set; }

        [JsonProperty("TrailMid")]
        public double TrailMid { get; set; }

        [JsonProperty("TrailHigh")]
        public double TrailHigh { get; set; }

        [JsonProperty("WheelBase")]
        public long WheelBase { get; set; }

        [JsonProperty("WheelBaseMid")]
        public double WheelBaseMid { get; set; }

        [JsonProperty("WheelBaseHigh")]
        public double WheelBaseHigh { get; set; }

        [JsonProperty("ChainStayLength")]
        public double ChainStayLength { get; set; }

        [JsonProperty("ChainStayLengthMid")]
        public double ChainStayLengthMid { get; set; }

        [JsonProperty("ChainStayLengthHigh")]
        public long ChainStayLengthHigh { get; set; }

        [JsonProperty("BottomBracketDrop")]
        public double BottomBracketDrop { get; set; }

        [JsonProperty("BottomBracketDropMid")]
        public double BottomBracketDropMid { get; set; }

        [JsonProperty("BottomBracketDropHigh")]
        public double BottomBracketDropHigh { get; set; }

        [JsonProperty("Stack")]
        public double Stack { get; set; }

        [JsonProperty("StackMid")]
        public double StackMid { get; set; }

        [JsonProperty("StackHigh")]
        public double StackHigh { get; set; }

        [JsonProperty("Reach")]
        public double Reach { get; set; }

        [JsonProperty("ReachMid")]
        public double ReachMid { get; set; }

        [JsonProperty("ReachHigh")]
        public double ReachHigh { get; set; }

        [JsonProperty("StandOverHeight")]
        public double StandOverHeight { get; set; }

        [JsonProperty("StandOverHeightMid")]
        public double StandOverHeightMid { get; set; }

        [JsonProperty("StandOverHeightHigh")]
        public double StandOverHeightHigh { get; set; }

        [JsonProperty("HandlebarWidth")]
        public long HandlebarWidth { get; set; }

        [JsonProperty("StemLength")]
        public long StemLength { get; set; }

        [JsonProperty("CrankLength")]
        public long CrankLength { get; set; }

        [JsonProperty("CrankLengthDec")]
        public long CrankLengthDec { get; set; }

        [JsonProperty("WheelSizeAlt")]
        public string WheelSizeAlt { get; set; }

        [JsonProperty("SizeStart")]
        [Newtonsoft.Json.JsonConverter(typeof(ParseStringConverter))]
        public long SizeStart { get; set; }

        [JsonProperty("SizeEnd")]
        [Newtonsoft.Json.JsonConverter(typeof(ParseStringConverter))]
        public long SizeEnd { get; set; }

        [JsonProperty("Image")]
        public string Image { get; set; }

        [JsonProperty("SortOrder")]
        public long SortOrder { get; set; }
    }

    public partial class Technology
    {
        [JsonProperty("Id")]
        public long Id { get; set; }

        [JsonProperty("Name")]
        public string Name { get; set; }

        [JsonProperty("Description")]
        public string Description { get; set; }

        [JsonProperty("OverviewImage")]
        public string OverviewImage { get; set; }

        [JsonProperty("PopupImage")]
        public string PopupImage { get; set; }

        [JsonProperty("BannerImage")]
        public string BannerImage { get; set; }
    }

    public class SpecificationRow
    {
        [JsonPropertyName("RowId")]
        public int RowId { get; set; }

        [JsonPropertyName("SpecificationKey")]
        public string SpecificationKey { get; set; }

        [JsonPropertyName("SpecificationValue")]
        public string SpecificationValue { get; set; }
    }

    public class SpecificationsObject
    {
        [JsonPropertyName("SpecificationRows")]
        public List<SpecificationRow> SpecificationRows { get; set; }
    }

    internal class ParseStringConverter : Newtonsoft.Json.JsonConverter
    {
        public override bool CanConvert(Type t) => t == typeof(long) || t == typeof(long?);

        public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null) return null;
            var value = serializer.Deserialize<string>(reader);
            long l;
            if (Int64.TryParse(value, out l))
            {
                return l;
            }
            throw new Exception("Cannot unmarshal type long");
        }

        public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
        {
            if (untypedValue == null)
            {
                serializer.Serialize(writer, null);
                return;
            }
            var value = (long)untypedValue;
            serializer.Serialize(writer, value.ToString());
            return;
        }

        public static readonly ParseStringConverter Singleton = new ParseStringConverter();
    }

    #endregion
}
