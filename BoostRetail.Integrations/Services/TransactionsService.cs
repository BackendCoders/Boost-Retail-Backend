using BoostRetail.Integrations.SConnect.Data;
using BoostRetail.Integrations.SConnect.DTOs;
using BoostRetail.Integrations.SConnect.Extensions;
using Microsoft.Extensions.Configuration;
using System.Globalization;

namespace BoostRetail.Integrations.SConnect.Services
{
    public class TransactionsService
    {
        private readonly SConnectDbContext _ctx;
        private readonly InventoryService _inventory;
        private readonly LocationService _location;
        private readonly IConfiguration _config;

        public TransactionsService(SConnectDbContext ctx,
            InventoryService inventory, 
            LocationService location, IConfiguration config) 
        {
            _ctx = ctx;
            _inventory = inventory;
            _location = location;
            _config = config;
        }

        public async Task<List<TransactionResponseDto>> GetTransactions()
        {
            var vatRates = File.ReadAllLines(_config["CompanyFilePath"]);

            var vatLookup = new Dictionary<int, decimal>();

            for (var i = 11; i < vatRates.Length; i++)
            {
                var line = vatRates[i];

                if (line.Contains("COMPANY_VAT_1"))
                {
                    var rate = line.Split('=')[1].Trim();
                    var irate = Convert.ToDecimal(rate);
                    vatLookup.Add(1, irate); 
                }
                else if (line.Contains("COMPANY_VAT_2"))
                {
                    var rate = line.Split('=')[1].Trim();
                    var irate = Convert.ToDecimal(rate);
                    vatLookup.Add(2, irate);
                }
                else if (line.Contains("COMPANY_VAT_3"))
                {
                    var rate = line.Split('=')[1].Trim();
                    var irate = Convert.ToDecimal(rate);
                    vatLookup.Add(3, irate);
                }
                else if (line.Contains("COMPANY_VAT_4"))
                {
                    var rate = line.Split('=')[1].Trim();
                    var irate = Convert.ToDecimal(rate);
                    vatLookup.Add(4, irate);
                }
                else if (line.Contains("COMPANY_VAT_5"))
                {
                    var rate = line.Split('=')[1].Trim();
                    var irate = Convert.ToDecimal(rate);
                    vatLookup.Add(5, irate);
                }
                else if (line.Contains("COMPANY_VAT_6"))
                {
                    var rate = line.Split('=')[1].Trim();
                    var irate = Convert.ToDecimal(rate);
                    vatLookup.Add(6, irate);
                }
                else if (line.Contains("COMPANY_VAT_7"))
                {
                    var rate = line.Split('=')[1].Trim();
                    var irate = Convert.ToDecimal(rate);
                    vatLookup.Add(7, irate);
                }
                else if (line.Contains("COMPANY_VAT_8"))
                {
                    var rate = line.Split('=')[1].Trim();
                    var irate = Convert.ToDecimal(rate);
                    vatLookup.Add(8, irate);
                }
            }

            var parts = await _inventory.GetPartNosAndDescriptionBarcode();
            var partnos = parts.Select(o => o.partno).ToList();
            var alllocs = await _location.GetLocationIds();
            var locs = alllocs.Select(o => o.BranchId.ToString().PadLeft(2,'0')).ToList();


            var date = DateTime.Parse(_config["TransactionsStartDate"], CultureInfo.InvariantCulture);

            var data =  _ctx.Transactions
                .Where(t => t.DateAndTime > DateTime.Now.AddDays(-365) ) // optional optimization
                .AsEnumerable() // switch to LINQ to Objects
                .Where(t => partnos.Contains(t.PartNumber) && locs.Contains(t.Location))
                .OrderByDescending(t => t.DateAndTime).ToList();

            var lst = new List<TransactionResponseDto>();

            // group transactions on invoice number
            var invoices = data.GroupBy(o => o.InvoiceNumber).ToList();

            foreach (var invoice in invoices)
            {
                var sale = new TransactionResponseDto();
                sale.Lines = new List<TransactionLineDto>();
                sale.CreateTime = invoice.First().DateAndTime.ToIso8601String();
                sale.Archived = false;
                sale.Completed = true; // Assuming all transactions are completed
                sale.Archived  = false;
                sale.Voided = false;
                sale.UpdateTime = invoice.First().DateAndTime.ToIso8601String();
                sale.Id = invoice.Key;

                foreach (var item in invoice)
                {
                    var des = parts.Where(o => o.partno == item.PartNumber)
                                            .Select(o => o.description)
                                            .FirstOrDefault();
                    var sku = parts.Where(o => o.partno == item.PartNumber).Select(o => o.mpn).FirstOrDefault();
                    var barcode = parts.Where(o => o.partno == item.PartNumber).Select(o => o.barcode).FirstOrDefault();
                    var vatcode = parts.Where(o => o.partno == item.PartNumber).Select(o => o.vatCode).FirstOrDefault();
                    var vatrate = vatLookup.ContainsKey(vatcode) ? vatLookup[vatcode] : 0;

                    var line = new TransactionLineDto
                    {
                        Id = item.Id,
                        ItemId = item.PartNumber,
                        CalcTotal = item.Sell,
                        CustomerId = item.Customer,
                        UnitQuantity = item.Quantity,
                        UnitPrice = item.Cost,
                        CalcLineDiscount = 0, // Assuming no discount for now
                        Tax2Rate = vatrate,
                        Description = des,
                        Sku = sku,
                        Upc = barcode,
                        CustomSku = item.PartNumber,
                        CreateTime = item.DateAndTime.ToIso8601String(), 
                        UpdateTime = item.DateAndTime.ToIso8601String(), 
                        Ean = string.Empty, 
                        Timezone = "UTC-0:00"
                    };

                    sale.Lines.Add(line);
                }

                lst.Add(sale);
            }

            return lst;
        }
    }
}
