using BoostRetail.Integrations.SConnect.Data;
using BoostRetail.Integrations.SConnect.DTOs;
using BoostRetail.Integrations.SConnect.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Runtime.InteropServices;

namespace BoostRetail.Integrations.SConnect.Services
{
    public class InventoryService
    {
        private SConnectDbContext _ctx;
        private LocationService _location;
        private readonly IConfiguration _config;

        public InventoryService(SConnectDbContext ctx, LocationService location, IConfiguration config)
        {
            _ctx = ctx;
            _location = location;
            _config = config;
        }

        public async Task<List<(string mpn,string partno, string description, string barcode, int vatCode)>> GetPartNosAndDescriptionBarcode()
        {
            var code = _config["SupplierCode"];
            var parts = await _ctx.InventoryItems.Where(o => o.Current == true && o.Supplier1Code == code).ToListAsync();
            var result = parts.Select(o => (o.MfrPartNumber, o.PartNumber, o.Search1 + " " + o.Search2, o.Barcode, o.VatCode)).ToList();
            return result;
        }

        public async Task<List<InventoryResponseDto>> GetInventory()
        { 
            var locations = await _location.GetLocationIds();
            var stock = await GetStock();

            var mpns = stock.Select(o => o.MPN).Distinct().OrderBy(o => o).ToList(); //.Skip(offset).Take(limit).ToList();

            // for each part we need to get the details
            var code = _config["SupplierCode"];
            var parts = await _ctx.InventoryItems.Where(o=>o.Current == true && o.Supplier1Code == code).ToListAsync();
            
            parts = parts.Where(o => mpns.Contains(o.MfrPartNumber)).ToList();

            // now build results
            var results = new List<InventoryResponseDto>();

            foreach (var mpn in mpns) 
            { 
                var part = parts.Where(o => o.MfrPartNumber == mpn).FirstOrDefault();

                if (part != null)
                {
                    var obj = new InventoryResponseDto
                    {
                        Description = part.Search1 + " " + part.Search2,
                        ManufacturerSku = mpn,
                        CustomSku = mpn,
                        Manufacturer = part.Make,
                        Upc = part.Barcode,
                        Ean = string.Empty,
                        Category = part.CatA,
                        ModelYear = part.Year,
                        ItemId = part.PartNumber,
                        CreatedAt = part.CreatedOn.ToIso8601String(),
                        UpdatedAt = part.UpdatedOn.ToIso8601String(),
                        Serialized = false,
                        SellingPrice = part.StorePrice,
                        AvgCost = part.CostPrice
                    };

                    foreach (var location in locations)
                    {
                        obj.Qoh = stock.Where(o => o.MPN == mpn && o.LocationCode == location.BranchId).Select(o => o.Qty).First();
                        obj.ShopSymbol = location.SpecLocatorId;
                    }

                    results.Add(obj);
                }
            }

            return results.ToList();
        }

        private async Task<List<SBCStockRow>> GetStock()
        {
            var stockfile = _config["SBCStockFilePath"];
            var stockRows = new List<SBCStockRow>();
            var arr = File.ReadAllLines(stockfile);

            var locs = await _location.GetLocationIds();

            for (int index = 1; index < arr.Length; index++)
            {
                var line = arr[index];
                var data = line.Split(new[] { ',' }, StringSplitOptions.None);
                var qty = data[1].Trim();

                try
                {
                    if (qty.Contains("-"))
                    {
                        qty = "-" + qty.Substring(0, qty.Length - 1).Trim();
                    }
                    qty = qty.Trim().Length > 0 ? Convert.ToInt32(qty.Trim()).ToString() : "0";
                }
                catch (Exception)
                {
                    qty = "0";
                }

                for (int i = 3; i < 31; i++)
                {
                    //var aa = arr[i];
                    if (data[i].Contains("-"))
                    {
                        data[i] = "-" + data[i].Substring(0, data[i].Length - 1).Trim();
                    }

                    try
                    {
                        data[i] = data[i].Trim().Length > 0 ? Convert.ToInt32(data[i].Trim()).ToString() : 0.ToString();
                    }
                    catch (Exception ex)
                    {
                        // Logger.Error($"Error converting data to correct format - value: {data[i]}",ex);
                        data[i] = "0";
                    }
                }



                for (int i = 1; i <= 28; i++)
                {
                    var qtyString = data[i + 2]; // Shift by 2 because data[0] = MPN, data[1-2] might be something else?
                    if (!int.TryParse(qtyString, out int qty1))
                    {
                        qty1 = 0; // Default to 0 if not parsable
                    }

                    if (locs.Exists(o => o.BranchId == i))
                    {
                        stockRows.Add(new SBCStockRow
                        {
                            MPN = data[0].Trim(),
                            LocationCode = i,
                            Qty = qty1
                        });
                    }
                }
            }

            return stockRows.OrderBy(o => o.MPN).ToList();
        }
    }
}
