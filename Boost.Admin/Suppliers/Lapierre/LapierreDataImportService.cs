using OfficeOpenXml;
using Serilog;
using Boost.Admin.Data.Models.Catalog;
using System.Net;

namespace SIM.Suppliers.Lapierre
{
    public class LapierreDataImportService
    {
        private readonly string FileLocationUrl = "https://cdn.abacusonline.net/SUPPLIER-DATA/Lapierre/MY24-Lapierre-Data.xlsx";
        private const string LocalFile = "_temp\\lapierre.xlsx";
        private readonly Serilog.ILogger _logger;

        public LapierreDataImportService()
        {
            _logger = Log.ForContext<LapierreDataImportService>();
        }

        public List<CatalogueItem> GetBikesFromFeed()
        {
            using (WebClient wc = new())
            {
                if (!Directory.Exists("_temp"))
                {
                    Directory.CreateDirectory("_temp");
                }

                // download and save to local file  
                _logger.Information($"downloading bikes file to local");
                wc.DownloadFile(FileLocationUrl, LocalFile);
            }

            if (!File.Exists(LocalFile))
                throw new FileNotFoundException(LocalFile);

            var feed = new List<LapierreDto>();

            // Ensure EPPlus is licensed to avoid LicenseException
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            _logger.Information($"importing data from file.");
            using (var package = new ExcelPackage(new FileInfo(LocalFile)))
            {
                var worksheet = package.Workbook.Worksheets["MY24 LP DATA"];
                var rowCount = worksheet.Dimension.Rows;

                for (int row = 2; row <= rowCount; row++) // Assuming the first row is the header
                {
                    var bicycle = new LapierreDto
                    {
                        SKU = worksheet.Cells[row, 1].Text,
                        Brand = worksheet.Cells[row, 2].Text,
                        ModelName = worksheet.Cells[row, 3].Text,
                        ImageLink = worksheet.Cells[row, 4].Text,
                        Barcode = worksheet.Cells[row, 5].Text,
                        Frame = worksheet.Cells[row, 6].Text,
                        Fork = worksheet.Cells[row, 7].Text,
                        Shock = worksheet.Cells[row, 8].Text,
                        FrontDerailleur = worksheet.Cells[row, 9].Text,
                        RearDerailleur = worksheet.Cells[row, 10].Text,
                        Shifters = worksheet.Cells[row, 11].Text,
                        Cassette = worksheet.Cells[row, 12].Text,
                        Chain = worksheet.Cells[row, 13].Text,
                        Crankset = worksheet.Cells[row, 14].Text,
                        ChainGuide = worksheet.Cells[row, 15].Text,
                        BottomBracket = worksheet.Cells[row, 16].Text,
                        Brakes = worksheet.Cells[row, 17].Text,
                        Rotors = worksheet.Cells[row, 18].Text,
                        Handlebar = worksheet.Cells[row, 19].Text,
                        Stem = worksheet.Cells[row, 20].Text,
                        Grips = worksheet.Cells[row, 21].Text,
                        Headset = worksheet.Cells[row, 22].Text,
                        Seatpost = worksheet.Cells[row, 23].Text,
                        Saddle = worksheet.Cells[row, 24].Text,
                        FrontHub = worksheet.Cells[row, 25].Text,
                        RearHub = worksheet.Cells[row, 26].Text,
                        Rims = worksheet.Cells[row, 27].Text,
                        Wheelset = worksheet.Cells[row, 28].Text,
                        Tires = worksheet.Cells[row, 29].Text,
                        Pedals = worksheet.Cells[row, 30].Text,
                        Accessories = worksheet.Cells[row, 31].Text,
                        SRP = decimal.Parse(worksheet.Cells[row, 32].Text)
                    };

                    feed.Add(bicycle);
                }
            }

            _logger.Information($"importing data from file and convert to dto complete.");
            return null;
        }
    }
}
