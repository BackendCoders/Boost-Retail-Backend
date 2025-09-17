using CsvHelper.Configuration;
using CsvHelper;
using Serilog;
using Boost.Admin.Configuration;
using System.Globalization;
using System.Net;
using System.Text;
using Boost.Admin.Data;
using HtmlAgilityPack;
using Boost.Admin.DTOs;
using Newtonsoft.Json;
using Boost.Admin.Data.Models.Catalog;

namespace SIM.Suppliers.Sportline
{
    public class SportlineDataImportService
    {
        private readonly string FileLocationUrl = "https://cdn.abacusonline.net/SUPPLIER-DATA/Sportline/M2035_ProductExport.csv";
        private const string LocalFile = "_temp\\sportline.csv";
        private readonly Serilog.ILogger _logger;

        public SportlineDataImportService()
        {
            _logger = Log.ForContext<SportlineDataImportService>();
        }

        public List<CatalogueItem> GetBikes()
        {
            var feed = GetFeed();

            _logger.Information($"converting feed to catalogue items");
            var items = ConvertProducts(feed);

            return items;
        }

        private List<CatalogueItem> ConvertProducts(List<SportlineFeedDto> items)
        {
            var res = new List<CatalogueItem>();

            foreach (var item in items)
            {
                var boxQty = 1;

                var spec = new SpecDto();
                var specJson = string.Empty;

                if(!string.IsNullOrEmpty(item.Specification))
                    spec = BuildSpecs(item.Specification);

                if (spec.Specs.Count > 0)
                    specJson = JsonConvert.SerializeObject(spec);

                ProductType type = ProductType.Accessory;

                if (item.Category == "Adventure" || item.Category == "Ridgeback")
                    type = ProductType.Bike;

                var obj = new CatalogueItem
                {
                    MPN = item.ProductCode,
                    Barcode = item.Barcode,
                    BoxQty = boxQty,
                    ProductTitle = item.ShortDescription,
                    ShortDescription = item.Description,
                    Brand = item.Brand,
                    Supplier = DataSupplier.Sportline,
                    GroupName = item.VariantGrouping,
                    Colour = ColorConverter.GetStandardColor(item.BrandColour),
                    Size = SizeConverter.GetStandardSize(item.Size),
                    Categorys = string.Empty,
                    Price = item.RRP,
                    SalePrice = 0,
                    Cost = item.TradePrice,
                    Year = 2024,
                    ProductType = type,
                    GenderOrAge = GenderOrAgeGroup.Unisex,
                    GeometryJson = string.Empty,
                    SpecificationsJson = specJson,
                    SupplierDetailsUrl = string.Empty
                };

                // deal with long description
                obj.LongDescription = item.LongWebText;

                // deal with images
                obj.Images = BuildImages(item);

                res.Add(obj);
            }

            return res;
        }

        private List<SportlineFeedDto> GetFeed()
        {
            var lst = new List<SportlineFeedDto>();

            using (WebClient wc = new())
            {
                if (!Directory.Exists("_temp"))
                {
                    Directory.CreateDirectory("_temp");
                }

                // download and save to local file  
                _logger.Information($"downloading madison file to local");
                wc.DownloadFile(FileLocationUrl, LocalFile);
            }

            if (!File.Exists(LocalFile))
                throw new FileNotFoundException(LocalFile);

            _logger.Information($"converting file feed");
            StreamReader reader;
            try
            {
                reader = new StreamReader(LocalFile, new UTF8Encoding());
            }
            catch (Exception ex)
            {
                throw new Exception("Error while reading stream.", ex);
            }

            var config = new CsvConfiguration(CultureInfo.InvariantCulture);
            config.BadDataFound = null;

            using (var csv = new CsvReader(reader, config))
            {
                csv.Context.RegisterClassMap<SportlineFeedMap>();
                var records = csv.GetRecords<SportlineFeedDto>();
                lst.AddRange(records);
            }
            reader.Dispose();

            lst.RemoveAll(o => o == null);
            return lst;
        }

        private string BuildImages(SportlineFeedDto obj)
        {
            var str = string.Empty;

            var images = new List<string>();
            var url = Constants.HostedImageUrlBase + "sportline/images/";

            if(!string.IsNullOrEmpty(obj.ImageName))
                images.Add($"{url}{obj.ImageName}.jpg");
            if (!string.IsNullOrEmpty(obj.AlternativeImage1))
                images.Add($"{url}{obj.AlternativeImage1}.jpg");
            if (!string.IsNullOrEmpty(obj.AlternativeImage2))
                images.Add($"{url}{obj.AlternativeImage2}.jpg");
            if (!string.IsNullOrEmpty(obj.AlternativeImage3))
                images.Add($"{url}{obj.AlternativeImage3}.jpg");

            images = images.Where(o => !string.IsNullOrEmpty(o)).ToList();

            if (images.Count == 1)
                str = images[0];
            else if (images.Count > 1)
                str = string.Join(",", images);

            return str;
        }

        private SpecDto BuildSpecs(string html)
        {
            var res = new SpecDto();
            var specs = new List<KeyValueDto>();
            var doc = new HtmlDocument();
            doc.LoadHtml(html);

            var rows = doc.DocumentNode.SelectNodes("//table[@class='mc-spec-table']/tr");

            if (rows != null)
            {
                foreach (var row in rows)
                {
                    var headerCell = row.SelectSingleNode("th");
                    var valueCell = row.SelectSingleNode("td");

                    if (headerCell != null && valueCell != null)
                    {
                        string key = headerCell.InnerText.Trim();
                        string value = valueCell.InnerText.Trim();
                        specs.Add(new KeyValueDto { Title = key, Value = value });
                    }
                }
            }

            res.Specs = specs;

            return res;
        }
    }
}
