using Newtonsoft.Json;
using OfficeOpenXml;
using Serilog;
using Boost.Admin.Data;
using Boost.Admin.Data.Models.Catalog;
using Boost.Admin.DTOs;
using System.Data;
using System.Net;


namespace SIM.Suppliers.Cube
{
    public class CubeDataImportService
    {
        private readonly Serilog.ILogger _logger;

        // 2023
        private const string BikeFeed23Url = "https://cdn.abacusonline.net/SUPPLIER-DATA/cube/2024-CUBE-7_BIKES-V1.2.xlsx";
        private const string EBikeFeed23Url = "https://cdn.abacusonline.net/SUPPLIER-DATA/cube/2024-CUBE-8_BIKES-V1.6.xlsx";
        private const string PAFeed23Url = "https://cdn.abacusonline.net/SUPPLIER-DATA/cube/2024-CUBE-PAC-V1.1.xlsx";

        // 2024
        private const string BikeFeed24Url = "https://cdn.abacusonline.net/SUPPLIER-DATA/cube/2024-CUBE-7_BIKES-V1.2.xlsx";
        private const string EBikeFeed24Url = "https://cdn.abacusonline.net/SUPPLIER-DATA/cube/2024-CUBE-8_BIKES-V1.6.xlsx";
        private const string PAFeed24Url = "https://cdn.abacusonline.net/SUPPLIER-DATA/cube/2024-CUBE-PAC-V1.1.xlsx";

        // 2025
        private const string BikeFeed25Url = "https://cdn.abacusonline.net/SUPPLIER-DATA/cube/2025-CUBE-BIKES.xlsx";
        private const string EBikeFeed25Url = "https://cdn.abacusonline.net/SUPPLIER-DATA/cube/2025-CUBE-EBIKES.xlsx";
        //private const string PAFeed25Url = "https://cdn.abacusonline.net/SUPPLIER-DATA/cube/2024-CUBE-PAC-V1.1.xlsx";


        private const string BikesLocalFile = "_temp\\cube-bikes.xlsx";
        private const string EBikesLocalFile = "_temp\\cube-ebikes.xlsx";
        private const string PALocalFile = "_temp\\cube-pa.xlsx";

        public CubeDataImportService()
        {
            _logger = Log.ForContext<CubeDataImportService>();
        }

        public List<CatalogueItem> GetPartsAccessories()
        {
            using (WebClient wc = new())
            {
                if (!Directory.Exists("_temp"))
                {
                    Directory.CreateDirectory("_temp");
                }

                // download and save to local file  
                _logger.Information($"downloading pa file to local");
                wc.DownloadFile(PAFeed24Url, PALocalFile);
            }

            if (!File.Exists(PALocalFile))
                throw new FileNotFoundException(PALocalFile);

            _logger.Information($"converting cube pa from local file to datatable.");
            var dt = ReadXlsx(PALocalFile);

            _logger.Information($"converting datatable to catalogue items");
            var pa = ConvertToPAFeed(dt);
            _logger.Information($"conversion complete");


            _logger.Information($"converting pa feed to catalgue items");
            var resPA = ConvertPAProducts(pa);
            _logger.Information($"convert pa complete");


            return resPA;
        }

        public List<CatalogueItem> GetBikesFromFeed()
        {
            var b2024 = Do2024Bikes();
            var b2025 = Do2025Bikes();

            return b2025;
        }

        private List<CatalogueItem> Do2024Bikes()
        {
            using (WebClient wc = new())
            {
                if (!Directory.Exists("_temp"))
                {
                    Directory.CreateDirectory("_temp");
                }

                // download and save to local file  
                _logger.Information($"downloading bikes file to local");
                wc.DownloadFile(BikeFeed24Url, BikesLocalFile);
                _logger.Information($"downloading ebikes file to local");
                wc.DownloadFile(EBikeFeed24Url, EBikesLocalFile);
            }

            if (!File.Exists(BikesLocalFile))
                throw new FileNotFoundException(BikesLocalFile);

            _logger.Information($"converting cube bikes from local file to datatable.");
            var dt = ReadXlsx(BikesLocalFile);

            _logger.Information($"converting datatable to dto");
            var bikes = ConvertToBikeFeed(dt, false);
            _logger.Information($"conversion complete");

            if (!File.Exists(EBikesLocalFile))
                throw new FileNotFoundException(EBikesLocalFile);

            _logger.Information($"converting cube ebikes from local file to datatable.");
            var dt1 = ReadXlsx(EBikesLocalFile);

            _logger.Information($"converting datatable to catalogue items");
            var ebikes = ConvertToBikeFeed(dt1, true);
            _logger.Information($"conversion complete");


            _logger.Information($"converting bike feed to catalgue items");
            var resBikes = ConvertBikeProducts(bikes, 2024, false);
            _logger.Information($"convert bikes complete");

            _logger.Information($"converting ebike feed to catalgue items");
            var resEBikes = ConvertBikeProducts(ebikes, 2024, true);
            _logger.Information($"complete");

            var res = new List<CatalogueItem>();

            res.AddRange(resBikes);
            res.AddRange(resEBikes);

            return res;
        }

        private List<CatalogueItem> Do2025Bikes()
        {
            using (WebClient wc = new())
            {
                if (!Directory.Exists("_temp"))
                {
                    Directory.CreateDirectory("_temp");
                }

                // download and save to local file  
                _logger.Information($"downloading bikes file to local");
                wc.DownloadFile(BikeFeed25Url, BikesLocalFile);
                _logger.Information($"downloading ebikes file to local");
                wc.DownloadFile(EBikeFeed25Url, EBikesLocalFile);
            }

            if (!File.Exists(BikesLocalFile))
                throw new FileNotFoundException(BikesLocalFile);

            _logger.Information($"converting cube bikes from local file to datatable.");
            var dt = ReadXlsx(BikesLocalFile);

            _logger.Information($"converting datatable to dto");
            var bikes = ConvertToBikeFeed(dt, false);
            _logger.Information($"conversion complete");

            if (!File.Exists(EBikesLocalFile))
                throw new FileNotFoundException(EBikesLocalFile);

            _logger.Information($"converting cube ebikes from local file to datatable.");
            var dt1 = ReadXlsx(EBikesLocalFile);

            _logger.Information($"converting datatable to catalogue items");
            var ebikes = ConvertToBikeFeed(dt1, true);
            _logger.Information($"conversion complete");


            _logger.Information($"converting bike feed to catalgue items");
            var resBikes = ConvertBikeProducts(bikes, 2025, false);
            _logger.Information($"convert bikes complete");

            _logger.Information($"converting ebike feed to catalgue items");
            var resEBikes = ConvertBikeProducts(ebikes, 2025, true);
            _logger.Information($"complete");

            var res = new List<CatalogueItem>();

            res.AddRange(resBikes);
            res.AddRange(resEBikes);

            return res;
        }

        private List<CubePartsAccFeedDto> ConvertToPAFeed(DataTable dataTable)
        {
            var expectedColumns = 21;

            var list = new List<CubePartsAccFeedDto>();

            if (dataTable.Columns.Count == expectedColumns)
            {
                for (int i = 1; i < dataTable.Rows.Count; i++)
                {
                    var dr = dataTable.Rows[i];
                    var obj = new CubePartsAccFeedDto()
                    {
                        PartNumber = dr[0].ToString(),
                        ModelCode = dr[1].ToString(),
                        EAN = dr[2].ToString(),
                        ProductTitle = dr[3].ToString(),
                        Status = dr[4].ToString(),
                        ModelTitle = dr[5].ToString(),
                        Colour = dr[6].ToString(),
                        Brand = dr[7].ToString(),
                        Size = dr[8].ToString(),
                        ModelNumber = dr[9].ToString(),
                        OriginalColourName = dr[10].ToString(),
                        ProductFamily = dr[11].ToString(),
                        ProductClass = dr[12].ToString(),
                        ProductGroup = dr[13].ToString(),
                        ProductType = dr[14].ToString(),
                        Model = dr[15].ToString(),
                        Type = dr[16].ToString(),
                        DealerPriceUK2024 = dr[17].ToString(),
                        RRPUK2024 = dr[18].ToString(),
                        Features = dr[19].ToString(),
                        Description = dr[20].ToString()
                    };
                    list.Add(obj);
                }
            }
            else
            {
                throw new Exception("Invalid length of data table.");
            }

            return list;
        }

        private List<CubeBikeFeedDto> ConvertToBikeFeed(DataTable dataTable, bool ebikes)
        {
            var expectedColumns = 0;

            if(!ebikes)
                expectedColumns = 73;
            else
                expectedColumns = 74;

            var list = new List<CubeBikeFeedDto>();

            if (dataTable.Columns.Count == expectedColumns)
            {
                for (int i = 1; i < dataTable.Rows.Count; i++)
                {
                    var dr = dataTable.Rows[i];
                    var obj = new CubeBikeFeedDto()
                    {
                        EANCode = dr[0].ToString(),
                        Scancode = dr[1].ToString(),
                        ItemCode = dr[2].ToString(),
                        ModelCode = dr[3].ToString(),
                        MotherCode = dr[4].ToString(),
                        Brand = dr[5].ToString(),
                        ProductDescription = dr[6].ToString(),
                        Modelname = dr[7].ToString(),
                        SizeCode = dr[8].ToString(),
                        SizeDescription = dr[9].ToString(),
                        Wheelsize = dr[10].ToString(),
                        Color = dr[11].ToString(),
                        Type = dr[12].ToString(),
                        BasicDealerPriceUK = dr[13].ToString(),
                        BasicConsumerPriceUK = dr[14].ToString(),
                        Description = dr[15].ToString(),
                        DescriptionFrame = dr[16].ToString(),
                        Frame = dr[17].ToString(),
                        Fork = dr[18].ToString(),
                        Shok = dr[19].ToString(),
                        ShokHardware = dr[20].ToString(),
                        Headset = dr[21].ToString(),
                        Stem = dr[22].ToString(),
                        Handlebar = dr[23].ToString(),
                        IntegratedBarStem = dr[24].ToString(),
                        BarExtensions = dr[25].ToString(),
                        HandlebarTape = dr[26].ToString(),
                        Grips = dr[27].ToString(),
                        RearDerailleur = dr[28].ToString(),
                        FrontDerailleur = dr[29].ToString(),
                        ChainGuide = dr[30].ToString(),
                        IntegratedShiftersBrakelevers = dr[31].ToString(),
                        Shifters = dr[32].ToString(),
                        BrakeLevers = dr[33].ToString(),
                        BrakeSystem = dr[34].ToString(),
                        BottomBracket = dr[35].ToString(),
                        Crankset = dr[36].ToString(),
                        CranksetChainringSizes = dr[37].ToString(),
                        Cassette = dr[38].ToString(),
                        GearTeeth = dr[39].ToString(),
                        NumberOfGears = dr[40].ToString(),
                        Chain = dr[41].ToString(),
                        WheelsetSystemWheels = dr[42].ToString(),
                        Rims = dr[43].ToString(),
                        FrontHub = dr[44].ToString(),
                        RearHub = dr[45].ToString(),
                        Tires = dr[46].ToString(),
                        FrontTire = dr[47].ToString(),
                        RearTire = dr[48].ToString(),
                        Pedals = dr[49].ToString(),
                        Saddle = dr[50].ToString(),
                        SeatPost = dr[51].ToString(),
                        SeatPostDropperYesNo = dr[52].ToString(),
                        SeatPostClamp = dr[53].ToString(),
                        FrontLight = dr[54].ToString(),
                        RearLight = dr[55].ToString(),
                        Kickstand = dr[56].ToString(),
                        Mudguard = dr[57].ToString(),
                        ChainProtection = dr[58].ToString(),
                        Bell = dr[59].ToString(),
                        Carrier = dr[60].ToString(),
                        Engine = dr[61].ToString(),
                        Battery = dr[62].ToString(),
                        Charger = dr[63].ToString(),
                        Display = dr[64].ToString(),
                        NetWeightKg = dr[65].ToString(),
                        GrossWeightKg = dr[66].ToString(),
                        BoxLabelCode = dr[67].ToString(),
                        BoxLength = dr[68].ToString(),
                        BoxWidth = dr[69].ToString(),
                        BoxHeight = dr[70].ToString(),
                        COO = dr[71].ToString(),
                        ImageURL = dr[72].ToString(),
                    };
                    list.Add(obj);
                }
            }
            else
            {
                throw new Exception("Invalid length of data table.");
            }

            return list;
        }

        private List<CatalogueItem> ConvertBikeProducts(List<CubeBikeFeedDto> items, int year, bool ebike)
        {
            var res = new List<CatalogueItem>();

            foreach (var item in items)
            {
                var type = ProductType.Bike;

                if (ebike)
                    type = ProductType.EBike;

                double cost = 0;
                double rrp = 0;

                double.TryParse(item.BasicConsumerPriceUK.Replace("£",""), out rrp);
                double.TryParse(item.BasicDealerPriceUK.Replace("£", ""), out cost);

                var spec = new SpecDto();
                var geo = new List<KeyValueDto>();

                var specJson = string.Empty;
                var geoJson = string.Empty;
                
                spec = BuildSpecs(item);
                
                if (spec.Specs.Count > 0)
                    specJson = JsonConvert.SerializeObject(spec);

                var obj = new CatalogueItem
                {
                    MPN = item.ItemCode,
                    Barcode = item.EANCode,
                    BoxQty = 1,
                    ProductTitle = item.Modelname,
                    ShortDescription = item.ProductDescription,
                    Brand = item.Brand,
                    Supplier = DataSupplier.Cube,
                    GroupName = item.MotherCode,
                    Colour = ColorConverter.GetStandardColor(item.Color),
                    Size = SizeConverter.GetStandardSize(item.SizeDescription),
                    Categorys = item.Type,
                    Price = rrp,
                    SalePrice = 0,
                    Cost = cost,
                    Year = year,
                    ProductType = type,
                    GenderOrAge = GenderOrAgeGroup.Unisex,
                    GeometryJson = geoJson,
                    SpecificationsJson = specJson,
                    SupplierDetailsUrl = string.Empty
                };

                // deal with long description
                obj.LongDescription = BuildDescription(item);

                // deal with images
                obj.Images = item.ImageURL;

                res.Add(obj);
            }

            return res;
        }

        private List<CatalogueItem> ConvertPAProducts(List<CubePartsAccFeedDto> items)
        {
            var res = new List<CatalogueItem>();

            foreach (var item in items)
            {
                double cost = 0;
                double rrp = 0;

                double.TryParse(item.RRPUK2024.Replace("£", ""), out rrp);
                double.TryParse(item.DealerPriceUK2024.Replace("£", ""), out cost);


                var type = ProductType.Accessory;

                if (item.ProductGroup == "Clothing")
                    type = ProductType.Clothing;


                var obj = new CatalogueItem
                {
                    MPN = item.PartNumber,
                    Barcode = item.EAN,
                    BoxQty = 1,
                    ProductTitle = item.ProductTitle,
                    ShortDescription = item.Features,
                    Brand = item.Brand,
                    Supplier = DataSupplier.Cube,
                    GroupName = item.Model,
                    Colour = ColorConverter.GetStandardColor(item.Colour),
                    Size = SizeConverter.GetStandardSize(item.Size),
                    Categorys = item.Type,
                    Price = rrp,
                    SalePrice = 0,
                    Cost = cost,
                    Year = 2024,
                    ProductType = type,
                    GenderOrAge = GenderOrAgeGroup.Unisex,
                    GeometryJson = string.Empty,
                    SpecificationsJson = string.Empty,
                    SupplierDetailsUrl = string.Empty
                };

                // deal with long description
                obj.LongDescription = item.Description;

                // deal with images
                obj.Images = string.Empty;

                res.Add(obj);
            }

            return res;
        }

        private DataTable ReadXlsx(string path, bool hasHeader = true)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            DataTable tbl = new DataTable();
            using (var pck = new ExcelPackage())
            {
                using (var stream = File.OpenRead(path))
                {
                    pck.Load(stream);
                }
                var ws = pck.Workbook.Worksheets.First();
                foreach (var firstRowCell in ws.Cells[1, 1, 1, ws.Dimension.End.Column])
                {
                    tbl.Columns.Add(hasHeader
                        ? firstRowCell.Text
                        : string.Format("Column {0}", firstRowCell.Start.Column));
                }
                var startRow = hasHeader ? 2 : 1;
                for (int rowNum = startRow; rowNum <= ws.Dimension.End.Row; rowNum++)
                {
                    var wsRow = ws.Cells[rowNum, 1, rowNum, ws.Dimension.End.Column];
                    DataRow row = tbl.Rows.Add();
                    foreach (var cell in wsRow)
                    {
                        row[cell.Start.Column - 1] = cell.Text;
                    }
                }
            }
            return tbl;
        }

        private string BuildDescription(CubeBikeFeedDto item)
        {
            var str = string.Empty;

            str = $"<p>{item.Description}</p>";

            if (!string.IsNullOrEmpty(item.DescriptionFrame))
            {
                str += $"<p>{item.DescriptionFrame}</p>";
            }

            return str;
        }

        private SpecDto BuildSpecs(CubeBikeFeedDto item)
        {
            var res = new SpecDto();

            var specs = new List<KeyValueDto>
            {
                new KeyValueDto { Title = "Frame", Value = item.Frame },
                new KeyValueDto { Title = "Fork", Value = item.Fork},
                new KeyValueDto { Title = "Shock", Value = item.Shok },
                new KeyValueDto { Title = "Shock Hardware", Value = item.ShokHardware },

                new KeyValueDto { Title = "Display", Value = item.Display },
                new KeyValueDto { Title = "Engine", Value = item.Engine },
                new KeyValueDto { Title = "Battery", Value = item.Battery },
                new KeyValueDto { Title = "Charger", Value = item.Charger },

                new KeyValueDto { Title = "Headset", Value = item.Headset },
                new KeyValueDto { Title = "Stem", Value = item.Stem },
                new KeyValueDto { Title = "Handlebar", Value = item.Handlebar },
                new KeyValueDto { Title = "Grips", Value = item.Grips },
                new KeyValueDto { Title = "Rear Derailleur", Value = item.RearDerailleur },
                new KeyValueDto { Title = "Front Derailleur", Value = item.FrontDerailleur },
                new KeyValueDto { Title = "Shifters", Value = item.Shifters },
                new KeyValueDto { Title = "Brake System", Value = item.BrakeSystem },
                new KeyValueDto { Title = "Bottom Bracket", Value = item.BottomBracket },
                new KeyValueDto { Title = "Crankset", Value = item.Crankset },
                new KeyValueDto { Title = "Crankset Chaining Sizes", Value = item.CranksetChainringSizes },

                new KeyValueDto { Title = "Cassette", Value = item.Cassette },
                new KeyValueDto { Title = "Number of Gears", Value = item.NumberOfGears },
                new KeyValueDto { Title = "Chain", Value = item.Chain },
                new KeyValueDto { Title = "Chain Protection", Value = item.ChainProtection },
                new KeyValueDto { Title = "Wheelset (System Wheels)", Value = item.ShokHardware },
                new KeyValueDto { Title = "Rims", Value = item.Rims },
                new KeyValueDto { Title = "Front Hub", Value = item.FrontHub },
                new KeyValueDto { Title = "Rear Hub", Value = item.RearHub },
                new KeyValueDto { Title = "Tyres", Value = item.Tires },
                new KeyValueDto { Title = "Front Tyre", Value = item.FrontTire },
                new KeyValueDto { Title = "Rear Tyre", Value = item.RearTire },
                new KeyValueDto { Title = "Saddle", Value = item.Saddle },
                new KeyValueDto { Title = "Pedals", Value = item.Pedals },

                new KeyValueDto { Title = "Seat Post", Value = item.SeatPost },
                new KeyValueDto { Title = "Seat Post Dropper", Value = item.SeatPostDropperYesNo },
                new KeyValueDto { Title = "Seat Post Clamp", Value = item.SeatPostClamp },

                new KeyValueDto { Title = "Front Light", Value = item.FrontLight },
                new KeyValueDto { Title = "Rear Light", Value = item.RearLight },
                new KeyValueDto { Title = "Kickstand", Value = item.Kickstand },
                new KeyValueDto { Title = "Mudguard", Value = item.Mudguard }
            };

              // remove emptys
            specs = specs.Where(o => !string.IsNullOrEmpty(o.Value)).ToList();
            res.Specs = specs;

            return res;
        }
    }
}
