using System.Xml.Linq;
using System.Xml.Serialization;

namespace AbacusOnline.SIM.Specialized
{
    #region XML CLASSES

    [Serializable]
    [XmlRoot(ElementName = "GEO")]
    public class GEO
    {
        public static List<GEO> Read(Stream stream)
        {
            var serializer = new XmlSerializer(typeof(List<GEO>), new XmlRootAttribute("GEOS"));
            return ((List<GEO>)serializer.Deserialize(stream));
        }

        [XmlElement(ElementName = "PRODUCT_SIZE")]
        public string ProductSize { get; set; }

        [XmlElement(ElementName = "MEASUREMENT")]
        public string Measurement { get; set; }

        [XmlElement(ElementName = "VALUE")]
        public string Value { get; set; }
    }

    [Serializable]
    [XmlRoot(ElementName = "SPECIFICATION")]
    public class SPECIFICATION
    {
        public static List<SPECIFICATION> Read(Stream stream)
        {
            var serializer = new XmlSerializer(typeof(List<SPECIFICATION>), new XmlRootAttribute("SPECIFICATIONS"));
            return ((List<SPECIFICATION>)serializer.Deserialize(stream));
        }

        [XmlElement(ElementName = "SPEC_NAME")]
        public string SpecName { get; set; }

        [XmlElement(ElementName = "SPEC_VALUE")]
        public string SpecValue { get; set; }
    }

    [Serializable]
    [XmlRoot(ElementName = "FEATURE")]
    public class FEATURE
    {
        public static List<FEATURE> Read(Stream stream)
        {
            var serializer = new XmlSerializer(typeof(List<FEATURE>), new XmlRootAttribute("PRODUCT_FEATURES"));
            return ((List<FEATURE>)serializer.Deserialize(stream));
        }

        [XmlElement(ElementName = "BULLET_TEXT")]
        public string BulletText { get; set; }
    }

    [Serializable]
    [XmlRoot(ElementName = "ITEM")]
    public class ITEM
    {
        public static List<ITEM> Read(Stream stream)
        {
            var serializer = new XmlSerializer(typeof(List<ITEM>), new XmlRootAttribute("ITEMS"));
            return ((List<ITEM>)serializer.Deserialize(stream));
        }

        [XmlElement(ElementName = "ITEM_CODE")]
        public string ItemCode { get; set; }

        [XmlElement(ElementName = "ITEM_DESCRIPTION")]
        public string ItemDescription { get; set; }

        [XmlElement(ElementName = "LONG_DESCRIPTION")]
        public string LongDescription { get; set; }

        [XmlElement(ElementName = "COLOR")]
        public string Color { get; set; }

        [XmlElement(ElementName = "SIZE")]
        public string Size { get; set; }

        [XmlElement(ElementName = "CLASS")]
        public string Class { get; set; }

        [XmlElement(ElementName = "FAMILY")]
        public string Family { get; set; }

        [XmlElement(ElementName = "SUB_FAMILY")]
        public string SubFamily { get; set; }

        [XmlElement(ElementName = "IMAGE_ID")]
        public string ImageId { get; set; }

        [XmlElement(ElementName = "PRODUCT_TYPE")]
        public string ProductType { get; set; }

        [XmlElement(ElementName = "RETAIL_PRICE")]
        public string RetailPrice { get; set; }

        [XmlElement(ElementName = "OFFER_PRICE")]
        public string OfferPrice { get; set; }

        [XmlElement(ElementName = "MODEL_YEAR")]
        public string ModelYear { get; set; }

        [XmlElement(ElementName = "PRODUCT_ID")]
        public string ProductId { get; set; }

        [XmlIgnore]
        public string FeaturesXml { get; set; }

        public string FeaturesToHtml()
        {
            if (!string.IsNullOrEmpty(FeaturesXml))
            {
                XDocument xmlDocument = XDocument.Parse(FeaturesXml);

                var result = new XDocument
                    (new XElement("ul",
                                from feature in xmlDocument.Descendants("FEATURE")
                                select new XElement("li", new XElement("td", feature.Element("BULLET_TEXT").Value)
                                            )));

                var res = result.ToString();
                return res;
            }
            return string.Empty;
        }

        [XmlIgnore]
        public List<GEO> Geometry { get; set; }
        public string GeometryToHtml()
        {
            if (Geometry != null && Geometry.Count > 0)
            {
                var grp = Geometry.GroupBy(o => o.Measurement).ToList();
                var sizes = Geometry.Select(o => o.ProductSize).Distinct();

                var html = "<table border='1'>";
                html += "<tr>";
                html += "<th>Size</th>";
                foreach (var s in sizes)
                {
                    html += "<th>" + s + "</th>";
                }
                html += "</tr>";
                foreach (var row in grp)
                {
                    html += "<tr>";
                    html += "<td>" + row.Key + "</td>";
                    foreach (var col in row)
                    {
                        html += "<td>" + col.Value + "</td>";
                    }
                    html += "</tr>";
                }

                html += "</table>";
                return html;
            }
            return string.Empty;
        }

        [XmlIgnore]
        public string SpecificationXml { get; set; }
        public string SpecificationToHtml()
        {
            if (!string.IsNullOrEmpty(SpecificationXml))
            {
                XDocument xmlDocument = XDocument.Parse(SpecificationXml);

                var result = new XDocument
                    (new XElement("table", new XAttribute("border", 1),
                            new XElement("thead",
                                new XElement("tr",
                                    new XElement("th", "Part"),
                                    new XElement("th", "Detail"))),
                            new XElement("tbody",
                                from spec in xmlDocument.Descendants("SPECIFICATION")
                                select new XElement("tr",
                                            new XElement("td", spec.Element("SPEC_NAME").Value),
                                            new XElement("td", spec.Element("SPEC_VALUE").Value)
                                            ))));

                var res = result.ToString();
                return res;
            }
            return string.Empty;
        }

        [XmlIgnore]
        public string ImageUrl { get; set; }
        [XmlIgnore]
        public string ImageHighDefUrl { get; set; }
    }

    [Serializable]
    [XmlRoot(ElementName = "PRODUCT")]
    public class PRODUCT
    {
        public static List<PRODUCT> Read(Stream stream)
        {
            var serializer = new XmlSerializer(typeof(List<PRODUCT>), new XmlRootAttribute("PRODUCTS"));
            return ((List<PRODUCT>)serializer.Deserialize(stream));
        }

        [XmlElement(ElementName = "BIKE_EQIP")]
        public string BikeEquip { get; set; }

        [XmlElement(ElementName = "MODEL_YEAR")]
        public string ModelYear { get; set; }

        [XmlElement(ElementName = "PRODUCT_ID")]
        public string ProductId { get; set; }

        [XmlElement(ElementName = "DISPLAY_NAME")]
        public string DisplayName { get; set; }

        [XmlElement(ElementName = "LONG_DESCRIPTION")]
        public string LongDescription { get; set; }

        [XmlElement(ElementName = "DISPLAY_ORDER")]
        public int DisplayOrder { get; set; }

        [XmlElement(ElementName = "IMAGE_ID")]
        public string ImageId { get; set; }

        public List<ITEM> ProductItems { get; set; }
    }

    [Serializable]
    [XmlRoot(ElementName = "SECTION")]
    public class SECTION
    {
        public static List<SECTION> Read(Stream stream)
        {
            var serializer = new XmlSerializer(typeof(List<SECTION>), new XmlRootAttribute("SECTIONS"));
            return ((List<SECTION>)serializer.Deserialize(stream));
        }

        [XmlElement(ElementName = "DISPLAY_NAME")]
        public string DisplayName { get; set; }

        [XmlElement(ElementName = "HEADER_TEXT")]
        public string HeaderText { get; set; }

        [XmlElement(ElementName = "SECTION_ID")]
        public string SectionId { get; set; }

        [XmlElement(ElementName = "IMAGE")]
        public string Image { get; set; }

        [XmlElement(ElementName = "DISPLAY_ORDER")]
        public int DisplayOrder { get; set; }

        [XmlElement(ElementName = "DISPLAY_TEXT")]
        public string DisplayText { get; set; }
    }

    [Serializable]
    [XmlRoot(ElementName = "ITEMAVAIL")]
    public class ITEMAVAIL
    {
        public static List<ITEMAVAIL> Read(Stream stream)
        {
            var serializer = new XmlSerializer(typeof(List<ITEMAVAIL>), new XmlRootAttribute("ITEMAVAILLIST"));
            return ((List<ITEMAVAIL>)serializer.Deserialize(stream));
        }

        [XmlElement(ElementName = "ITEM")]
        public string ITEM { get; set; }
        [XmlElement(ElementName = "AVAIL")]
        public string AVAIL { get; set; }
        [XmlElement(ElementName = "NEXTDATE")]
        public string NEXTDATE { get; set; }
        [XmlElement(ElementName = "RETAILPRICE")]
        public string RETAILPRICE { get; set; }
        [XmlElement(ElementName = "OFFERPRICE")]
        public string OFFERPRICE { get; set; }
        [XmlElement(ElementName = "OLDPRICE")]
        public string OLDPRICE { get; set; }
        [XmlElement(ElementName = "SELLINGPRICE")]
        public string SELLINGPRICE { get; set; }
    }

    #endregion

    #region STOCK CLASSES
    [Serializable]
    [XmlRoot(ElementName = "ITEMAVAILLIST")]
    public class ITEMAVAILLIST
    {
        [XmlElement(ElementName = "ITEMAVAIL")]
        public List<ITEMAVAIL> ITEMAVAIL { get; set; }

        public void Read(Stream stream)
        {
            var serializer = new XmlSerializer(typeof(List<ITEMAVAIL>), new XmlRootAttribute("ITEMAVAILLIST"));
            ITEMAVAIL = (List<ITEMAVAIL>)serializer.Deserialize(stream);
        }
    }
    #endregion


    public class Family
    {
        public Family()
        {
            Ranges = new List<PRODUCT>();
        }
        public string SectionName { get; set; }
        public string SectionId { get; set; }
        public string DisplayText { get; set; }
        public List<PRODUCT> Ranges { get; set; }

        public int GetTotalProducts()
        {
            return Ranges.Sum(range => range.ProductItems.Count);
        }


    }

    public enum FamilyType
    {
        Bikes,
        Apparel,
        Equiptment
    }
}
