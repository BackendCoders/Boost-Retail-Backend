using CsvHelper.Configuration;
using CsvHelper;
using System.Globalization;
using System.Text;
using Boost.Admin.Configuration;
using Serilog;
using System.Net;
using Boost.Admin.Data;
using Boost.Admin.Data.Models.Catalog;

namespace SIM.Suppliers.Madison
{
    public class MadisonDataImportService
    {
        private readonly string FileLocationUrl = "https://cdn.abacusonline.net/SUPPLIER-DATA/Madison/MM2029.csv";
        private const string LocalFile = "_temp\\madison.csv";
        private readonly Serilog.ILogger _logger;

        public MadisonDataImportService()
        {
            _logger = Log.ForContext<MadisonDataImportService>();
        }

        public List<CatalogueItem> GetPartsAccessories()
        {
            var feed = GetFeed();

            _logger.Information($"converting feed to catalogue items");
            var items = ConvertProducts(feed);

            return items;
        }

        private List<CatalogueItem> ConvertProducts(List<MadisonFeedDto> items)
        { 
            var res = new List<CatalogueItem>();

            foreach (var item in items)
            {
                var boxQty = 1;

                if (item.UOM == "Each" || item.UOM == "Pack" || item.UOM == "Pair" || item.UOM == "Set" || item.UOM == "Kit")
                {
                    boxQty = 1;
                }
                else if (item.UOM.StartsWith("Pairs"))
                {
                    var qtyStr = item.UOM.Replace(" Pairs", "").Trim();
                    var qty = 0;
                    int.TryParse(qtyStr, out qty);

                    boxQty = qty;
                }
                else if (item.UOM.StartsWith("Pack of ") || item.UOM.StartsWith("Box of ") || item.UOM.StartsWith("Set of "))
                {
                    var qtyStr = item.UOM.Replace("Pack of ", "").Replace("Box of ", "").Replace(" Pairs", "").Replace("Set of ","").Trim();
                    var qty = 0;
                    int.TryParse(qtyStr, out qty);

                    boxQty = qty;
                }

                var obj = new CatalogueItem
                {
                    MPN = item.Product,
                    Barcode = item.Barcode,
                    BoxQty = boxQty,
                    ProductTitle = item.Description,
                    ShortDescription = item.Description,
                    Brand = item.Brand,
                    Supplier = DataSupplier.Madison,
                    GroupName = item.Hierachy,
                    Colour = ColorConverter.GetStandardColor(item.Colour),
                    Size = SizeConverter.GetStandardSize(item.Size),
                    Categorys = string.Empty,
                    Price = item.SRP,
                    SalePrice = 0,
                    Cost = item.TradePrice,
                    Year = 2024,
                    ProductType = ProductType.Accessory,
                    GenderOrAge = GenderOrAgeGroup.Unisex,
                    GeometryJson = string.Empty,
                    SpecificationsJson = string.Empty,
                    SupplierDetailsUrl = string.Empty
                };

                // deal with long description
                obj.LongDescription = item.WebFriendlyDescription;

                // deal with images
                obj.Images = BuildImages(item);

                res.Add(obj);
            }

            return res;
        }

        private string BuildImages(MadisonFeedDto obj)
        {
            var str = string.Empty;

            var url = Constants.HostedImageUrlBase + "madison/images/";

            str = $"{url}{obj.ImageFilename}.jpg";

            return str;
        }

        private List<MadisonFeedDto> GetFeed()
        {
            var lst = new List<MadisonFeedDto>();

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
                csv.Context.RegisterClassMap<MadisonFeedMap>();
                var records = csv.GetRecords<MadisonFeedDto>();
                lst.AddRange(records);
            }
            reader.Dispose();

            lst.RemoveAll(o => o == null);
            return lst;
        }
    }
}
