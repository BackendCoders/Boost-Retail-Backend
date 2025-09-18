using Boost.Admin.Data;
using Boost.Admin.Data.Models.Catalog;
using Boost.Admin.Logic;
using Boost.Retail.Data.DTO;
using Microsoft.AspNetCore.Mvc;
using Serilog;


namespace SIMWORKER
{
    internal class ProcessCatalogueToProducts
    {
        private readonly ILogger _logger;
        private readonly ICatalogueLogic _catalogueLogic;
        private readonly IImportLogic _importLogic;

        public ProcessCatalogueToProducts(ICatalogueLogic logic, IImportLogic logic2)
        {
            _catalogueLogic = logic;
            _importLogic = logic2;
            _logger = Log.ForContext<ProcessCatalogueToProducts>();
        }

        public async Task<ActionResult> Exists(string mpn)
        {
            try
            {
                //return OkResult(); if no return object
                return new OkObjectResult(await _catalogueLogic.Exists(mpn));
            }
            catch (Exception ex)
            {
                return new ObjectResult(ex.Message)
                {
                    StatusCode = 500
                };
            }
        }

        public async Task ProcessItems(List<Boost.Admin.DTOs.SIMProductDto> items, DataSupplier supplier)
        {
            _logger.Information("begin filling foreign tables");

            // VAT RATES
            var vatList = await _importLogic.GetWithAddVatRates();

            // BRAND
            var brands = items.Select(x => x.Brand).Distinct().ToList();
            var brandList = await _importLogic.GetOrInsertBrands(brands);

            // COLOURS
            var colors = items.Select(x => x.Colour).Distinct().ToList();
            var colorList = await _importLogic.GetOrInsertColors(colors);

            // SIZES
            var sizes = items.Select(x => x.Size).Distinct().ToList();
            var sizeList = await _importLogic.GetOrInsertSizes(sizes);

            // GEOMETRY
            var geo = items.Select(x => x.GeometryJson).Distinct().ToList();
            var geoList = await _importLogic.GetOrInsertGeos(geo);

            // SPECIFICATIONS
            var spec = items.Select(x => x.SpecificationsJson).Distinct().ToList();
            var specList = await _importLogic.GetOrInsertSpecs(spec);

            // LONG DESCRIPTIONS
            var longDesc = items.Select(x => x.LongDescription).Distinct().ToList();
            var longDescList = await _importLogic.GetOrInsertLongDescriptions(longDesc);

            // SHORT DESCRIPTIONS
            var shortDesc = items.Select(x => x.ShortDescription).Distinct().ToList();
            var shortDescList = await _importLogic.GetOrInsertShortDescriptions(shortDesc);

            var newList = new List<MasterProduct>();

            foreach (var item in items)
            {
                if (item.VatRate == null)
                    item.VatRate = 0;

                var nobj = new MasterProduct();

                nobj.MPN = item.MPN;
                nobj.Barcode = item.Barcode;
                nobj.BoxQty = item.BoxQty;
                nobj.Price = item.Price;
                nobj.GenderOrAge = item.GenderOrAge;
                nobj.Cost = item.Cost;
                nobj.GroupName = item.GroupName;
                nobj.Images = item.Images;
                nobj.ProductType = item.ProductType;
                nobj.Status = ProductStatus.Deferred;
                nobj.SupplierDetailsUrl = item.SupplierDetailsUrl;
                nobj.Title = item.ProductTitle;
                nobj.VideoUrl = item.VideoUrl;
                nobj.Weight = item.Weight;
                nobj.Year = item.Year;

                nobj.Supplier = supplier;
                nobj.BrandId = brandList.First(o => o.Name == item.Brand).Id;
                nobj.ColourId = colorList.First(o => o.Name == item.Colour).Id;
                nobj.SizeId = sizeList.First(o => o.Name == item.Size).Id;
                nobj.VatRateId = vatList.First(o => o.Rate == (decimal)item.VatRate).Id;
                nobj.LongDescId = longDescList.First(o => o.Description == item.LongDescription).Id;
                nobj.ShortDescId = shortDescList.First(o => o.Description == item.ShortDescription).Id;
                nobj.GeoId = geoList.First(o => o.GeometryJson == item.GeometryJson).Id;
                nobj.SpecId = specList.First(o => o.SpecificationJson == item.SpecificationsJson).Id;

                newList.Add(nobj);
            }

            // assign our categories
            await AssignBikeCategories(newList, supplier);

            // assign group name
            await AssignUniqueGroupName(newList);

            // assign unique color code for group colors
            await AssignColorGroup(newList);

            // save the list
            foreach (var product in newList)
            {
                await _importLogic.Save(product);
            }
        }

        private async Task AssignUniqueGroupName(List<MasterProduct> products)
        {
            // Group by GroupName, then Year
            var grouped = products
                .GroupBy(p => p.GroupName)
                .SelectMany(groupByName => groupByName
                    .GroupBy(p => p.Year)
                    .Select(groupByYear => groupByYear.ToList())
                );

            // Assign new GroupName in the format: "Original_GroupName_Underscored_Year"
            foreach (var group in grouped)
            {
                var originalGroupName = group.First().GroupName;
                var year = group.First().Year;

                var uniqueGroupName = originalGroupName.Replace(" ", "_") + "_" + year;

                foreach (var product in group)
                {
                    product.GroupName = uniqueGroupName;
                }
            }

            await Task.CompletedTask;
        }

        private async Task AssignColorGroup(List<MasterProduct> products)
        {
            // Group by GroupName, then by ColorId, then by Year
            var grouped = products
                .GroupBy(p => p.GroupName)
                .SelectMany(groupByName => groupByName
                    .GroupBy(p => p.ColourId)
                    .Select(groupByColor => groupByColor).ToList()
                );

            // Assign a new ColorGroup Guid to each subgroup
            foreach (var group in grouped)
            {
                var colorGroupId = Guid.NewGuid();
                foreach (var product in group)
                {
                    product.ColorGroup = colorGroupId.ToString();
                }
            }

            await Task.CompletedTask;
        }

        private async Task AssignBikeCategories(List<MasterProduct> products, DataSupplier supplier)
        {
            string[] data = null;
            switch (supplier)
            {
                case DataSupplier.Giant:
                    data = File.ReadAllLines("Data\\Maps\\GIANT_BIKES_MODELS_LOOKUP_TABLE.csv")
                        .Concat(File.ReadAllLines("Data\\Maps\\LIV_BIKES_MODELS_LOOKUP_TABLE.csv"))
                        .ToArray();
                    break;
                case DataSupplier.Trek:
                    data = File.ReadAllLines("Data\\Maps\\TREK_BIKES_MODELS_LOOKUP_TABLE.csv");
                    break;
                case DataSupplier.Specialized:
                    data = File.ReadAllLines("Data\\Maps\\SPECIALZED_BIKES_MODELS_LOOKUP_TABLE.csv");
                    break;
                case DataSupplier.Raleigh:
                    data = File.ReadAllLines("Data\\Maps\\RALEIGH_BIKES_MODELS_LOOKUP_TABLE.csv")
                        .Concat(File.ReadAllLines("Data\\Maps\\HAIBIKE_BIKES_MODELS_LOOKUP_TABLE.csv"))
                        .Concat(File.ReadAllLines("Data\\Maps\\LAPIERRE_BIKES_MODELS_LOOKUP_TABLE.csv"))
                        .ToArray();
                    break;
                case DataSupplier.Whyte:
                    data = File.ReadAllLines("Data\\Maps\\WHYTE_BIKES_MODELS_LOOKUP_TABLE.csv");
                    break;
                case DataSupplier.Madison:
                    //data = File.ReadAllLines("Data\\Maps\\SPECIALZED_BIKES_MODELS_LOOKUP_TABLE.csv");
                    break;
                case DataSupplier.Cube:
                    data = File.ReadAllLines("Data\\Maps\\CUBE_BIKES_MODEL_LOOKUP_TABLE.csv");
                    break;
                case DataSupplier.Sportline:
                    //data = File.ReadAllLines("Data\\Maps\\SPECIALZED_BIKES_MODELS_LOOKUP_TABLE.csv");
                    break;
                default:
                    break;
            }


            var withBikes = new Dictionary<string, string>();
            var withEBikes = new Dictionary<string, string>();
            for (int i = 1; i < data.Length; i++)
            {
                var line = data[i].Trim();

                var a = line.Contains("Electric Bike");

                if (!a)
                {
                    try
                    {
                        withBikes.Add(line.Split(',')[1], line);
                    }
                    catch (Exception ex)
                    {
                        
                    }
                    
                }
                else
                {
                    withEBikes.Add(line.Split(',')[1], line);
                }
            }

            var final = new List<MasterProduct>();

            foreach (var model in withBikes)
            {
                if (model.Key == "EnviLiv") 
                {

                }

                var bikes = products.Where(o => o.Title.Contains(model.Key) && o.ProductType == ProductType.Bike).ToList();

                if (bikes.Count != 0)
                {
                    await Assign(bikes, model.Value);
                    final.AddRange(bikes);
                }
            }

            foreach (var model in withEBikes)
            {
                var ebikes = products.Where(o => o.Title.Contains(model.Key) && o.ProductType == ProductType.EBike).ToList();

                if (ebikes.Count != 0)
                {
                    await Assign(ebikes, model.Value);
                    final.AddRange(ebikes);
                }
            }

            var notfound = products.Where(o => !final.Select(x => x.MPN).Contains(o.MPN)).ToList();

            if (notfound.Count > 0)
            {
                var mlist = notfound.Select(x => new { x.Title,x.Brand, x.Supplier }).Distinct().ToList();

                var lst = new List<string>();

                foreach (var m in mlist) 
                {
                    lst.Add($"{m.Title},{m.Brand},{m.Supplier}");
                }

                //throw new Exception("Not all products were mapped");
                File.WriteAllLines(@$"{supplier}_NOT_FOUND_PRODUCTS.csv", lst);
            }
        }

        private async Task Assign(List<MasterProduct> products, string categorys)
        {
            var a = categorys.Split(',')[2];
            var b = categorys.Split(',')[3];
            var c = categorys.Split(',')[4];

            var cataid = await _importLogic.GetCategory(a);
            var catbid = await _importLogic.GetCategory(b);
            var catcid = await _importLogic.GetCategory(c);

            foreach (var product in products)
            {
                product.Category1Id = cataid?.Id ?? 0;
                product.Category2Id = catbid?.Id ?? 0;
                product.Category3Id = catcid?.Id ?? 0;
            }
        }
    }
}
