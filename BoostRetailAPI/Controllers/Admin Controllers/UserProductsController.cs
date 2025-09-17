using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Boost.Admin.Data;
using Boost.Admin.Data.Models;
using Boost.Admin.Data.Models.Catalog;
using Boost.Admin.DTOs;
using Boost.Retail.Data.Models;


namespace BoostRetailAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class UserProductsController : ControllerBase
    {
        private SimDbContext _db;
        private IMapper _mapper;

        public UserProductsController(SimDbContext db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        [HttpGet]
        [Route("GetUserProducts")]
        public async Task<IActionResult> GetUserProducts(string userid)
        {
            var selected = await _db.UserSupplierProductCategoryBrands
                .Where(o => o.UserId == userid).ToListAsync();

            var res = new List<MasterProduct>();

            foreach (var s in selected)
            {
                var items = await _db.MasterProducts
                    .Include(p => p.Brand)
                   .Include(p => p.Category1)
                   .Include(p => p.Category2)
                   .Include(p => p.Category3)
                   .Include(p => p.Colour)
                   .Include(p => p.Size)
                   .Include(p => p.Spec)
                   .Include(p => p.Geo)
                   .Include(p => p.VatRate)
                    .Where(o => 
                    o.Category2Id == s.Category2Id &&
                    o.Category3Id == s.Category3Id &&
                    o.BrandId == s.BrandId)
                    .ToListAsync();

                res.AddRange(items);
            }

            return Ok(res);
        }

        [HttpPost]
        [Route("UploadEposData")]
        public async Task<IActionResult> UploadEposData(EposFeed feed)
        {
            var uid = feed.UserId;

            ///
            /// filter out so we have only customer epos products that we dont have in Master
            /// 

            // get users master products range
            var selected = await _db.UserSupplierProductCategoryBrands
                .AsNoTracking()
               .Where(o => o.UserId == uid).ToListAsync();

            var master = new List<MasterProduct>();

            foreach (var s in selected)
            {
                var items = await _db.MasterProducts
                    .AsNoTracking()
                    .Where(o =>
                    o.Category2Id == s.Category2Id &&
                    o.Category3Id == s.Category3Id &&
                    o.BrandId == s.BrandId)
                    .ToListAsync();

                master.AddRange(items);
            }

            // convert master to user product
            var products = _mapper.Map<List<MasterProduct>, List<UserProduct>>(master);

            /// manually map the following:
            foreach (var product in products)
            { 
                // check we have this is epos and copy data
                var e = feed.EposProducts.Where(o=>o.MPN == product.MPN).FirstOrDefault();
                if (e != null) 
                {
                    product.Price = e.Price;
                    product.PromoPrice = e.PromoPrice;
                    product.PromoStart = e.PromoStart;
                    product.PromoEnd = e.PromoEnd;
                    product.EPN = e.PartNo;
                }
            }

            // create mpn list
            var mmpns = new HashSet<string>(products.Select(o=>o.MPN.Trim()));

            // remove what we have in products already
            var epos = feed.EposProducts.Where(o => !mmpns.Contains(o.MPN.Trim())).ToList();

            // interogate the master db to see if we have this mpn and get its data
            foreach (var part in epos)
            {
                var mpart = await _db.MasterProducts.AsNoTracking().Where(o => o.MPN == part.MPN).FirstOrDefaultAsync();

                if (mpart != null) // master product found
                {
                    // convert to user product
                    var userpart = _mapper.Map<MasterProduct, UserProduct>(mpart);

                    // take epos specific values
                    userpart.Price = part.Price;
                    userpart.PromoPrice = part.PromoPrice;
                    userpart.PromoStart = part.PromoStart;
                    userpart.PromoEnd = part.PromoEnd;
                    userpart.EPN = part.PartNo;

                    // merge to list
                    products.Add(userpart);
                }
                else 
                {
                    // map categorys for epos products
                }
            }

            // apply overides to master
            //var goverrides = _bdb.UserProductGroupOverrides
            //    .Where(o => o.UserId == feed.UserId).ToList();

            //var poverrides = _bdb.UserProductOverrides
            //    .Where(o => o.UserId == feed.UserId).ToList();

            //foreach (var o in goverrides) 
            //{
                
            //}

            // create report - added/removed/changed?

            return Ok();
        }

        [HttpPost, Route("SearchProducts")]
        public async Task<List<MasterProduct>> SearchProducts([FromBody] SearchDto search)
        {
            var year = search.Year;
            var supplier = search.Supplier;
            var brand = search.Brand;
            var group = search.Group;
            var type = search.Type;
            var minCost = search.MinCost;
            var maxCost = search.MaxCost;
            var minPrice = search.MinPrice;
            var maxPrice = search.MaxPrice;
            var minSalePrice = search.MinSalePrice;
            var maxSalePrice = search.MaxSalePrice;
            var skip = search.Skip;
            var take = search.Take;

            var valid = int.TryParse(year, out var yr);
            var lst = await _db.MasterProducts
                .AsNoTracking()
                .Include(o => o.Brand)
                .Include(o => o.ShortDescription)
                .Include(o => o.LongDescription)
                .Where(o => o.Supplier == supplier 
                && (o.Year == yr || year == "0") 
                && (o.Brand.Name == brand  || string.IsNullOrEmpty(brand))
                && (o.GenderOrAge == group || group == GenderOrAgeGroup.None)
                && (o.ProductType == type || type == ProductType.Unknown)
                && (o.Cost.GetValueOrDefault(0) >= minCost 
                && o.Cost.GetValueOrDefault(0) <= maxCost)
                && (o.Price >= minPrice && o.Price <= maxPrice)
                && (o.Price >= minSalePrice && o.Price <= maxSalePrice))

                //&& (o.Title.Contains(keyword, StringComparison.OrdinalIgnoreCase) ||
                //    o.ShortDescription.Description.Contains(keyword, StringComparison.OrdinalIgnoreCase) ||
                //    o.LongDescription.Description.Contains(keyword, StringComparison.OrdinalIgnoreCase) ||
                //    o.Brand.Name.Contains(keyword, StringComparison.OrdinalIgnoreCase)))
                .AsNoTracking()
                .Skip(skip)
                .Take(take)
                .ToListAsync();

            return lst;
        }

        [HttpPost, Route("SearchProductsCount")]
        public async Task<int> SearchProductsCount([FromBody] SearchDto search)
        {
            var year = search.Year;
            var supplier = search.Supplier;
            var brand = search.Brand;
            var group = search.Group;
            var type = search.Type;
            var minCost = search.MinCost;
            var maxCost = search.MaxCost;
            var minPrice = search.MinPrice;
            var maxPrice = search.MaxPrice;
            var minSalePrice = search.MinSalePrice;
            var maxSalePrice = search.MaxSalePrice;
            var skip = search.Skip;
            var take = search.Take;

            var valid = int.TryParse(year, out var yr);
            var lst = await _db.MasterProducts
                .AsNoTracking()
                .Include(o => o.Brand)
                .Include(o => o.ShortDescription)
                .Include(o => o.LongDescription)
                .Where(o => o.Supplier == supplier
                && (o.Year == yr || year == "0")
                && (o.Brand.Name == brand || string.IsNullOrEmpty(brand))
                && (o.GenderOrAge == group || group == GenderOrAgeGroup.None)
                && (o.ProductType == type || type == ProductType.Unknown)
                && (o.Cost.GetValueOrDefault(0) >= minCost
                && o.Cost.GetValueOrDefault(0) <= maxCost)
                && (o.Price >= minPrice && o.Price <= maxPrice)
                && (o.Price >= minSalePrice && o.Price <= maxSalePrice))

                //&& (o.Title.Contains(keyword, StringComparison.OrdinalIgnoreCase) ||
                //    o.ShortDescription.Description.Contains(keyword, StringComparison.OrdinalIgnoreCase) ||
                //    o.LongDescription.Description.Contains(keyword, StringComparison.OrdinalIgnoreCase) ||
                //    o.Brand.Name.Contains(keyword, StringComparison.OrdinalIgnoreCase)))
                .AsNoTracking()
                .CountAsync();

            return lst;
        }

       

        [HttpGet, Route("Suppliers")]
        public async Task<IActionResult> GetSuppliers()
        {
            var suppliers = await _db.MasterProducts
                .Select(o => o.Supplier)
                .Distinct()
                .ToListAsync();

        var result = suppliers
        .Select(s => new
        {
            Id = (int)s,
            Name = s.ToString()
        });

            return Ok(result);
        }

        [HttpGet, Route("Categories")]
        public async Task<List<string>> GetCategories()
        {
            var obj = await _db.MasterProducts
                .Where(o=> o.Category1!=null)
                .Select(o => o.Category1.Name)
                .Distinct()
                .ToListAsync();

            return obj;
        }

        [HttpGet, Route("Years")]
        public async Task<IActionResult> GetYears([FromQuery] DataSupplier supplier)
        {
            var years = await _db.MasterProducts
                    .Where(o => o.Supplier == supplier && o.Year != 0)
                    .Select(o => o.Year)
                    .Distinct()
                    .ToListAsync();
            return Ok(years);
        }

        [HttpGet, Route("Brands")]
        public async Task<IActionResult> GetBrands([FromQuery] DataSupplier supplier, string year)
        {
            var valid = int.TryParse(year, out var yr);

            if (valid)
            {
                var brands = await _db.MasterProducts
                    .Where(o => o.Supplier == supplier && (o.Year == yr || year == "0"))
                    .Select(o => o.Brand)
                    .Distinct()
                    .ToListAsync();

                var result = brands
                .Select(s => new
                {
                   s.Id,
                   s.Name
                });
                return Ok(result);
            }
            else
                throw new InvalidOperationException("Invalid year");
        }

        [HttpGet, Route("Groups")]
        public async Task<IActionResult> GetGroups([FromQuery] DataSupplier supplier, string year)
        {
            var valid = int.TryParse(year, out var yr);

            if (valid)
            {
                var groups = await _db.MasterProducts
                    .Where(o => o.Supplier == supplier && (o.Year == yr || year == "0"))
                    .Select(o => o.GroupName)
                    .Distinct()
                    .ToListAsync();
                return Ok(groups);
            }
            else
                throw new InvalidOperationException("Invalid year");
        }

        [HttpGet, Route("GroupsByBrand")]
        public async Task<IActionResult> GetGroupsByBrand([FromQuery] DataSupplier supplier, string year, string brand)
        {
            var valid = int.TryParse(year, out var yr);

            if (valid)
            {
                var groups = await _db.MasterProducts
                    .Where(o => o.Supplier == supplier && (o.Year == yr || year == "0") && o.Brand.Name.ToUpper()  == brand.ToUpper())
                    .Select(o => o.GroupName)
                    .Distinct()
                    .ToListAsync();
                return Ok(groups);
            }
            else
                throw new InvalidOperationException("Invalid year");
        }


        [HttpGet, Route("ProductCountSupplier")]
        public async Task<int> GetProductCountBySupplier([FromQuery] DataSupplier supplier)
        {
            var obj = await _db.MasterProducts
                    .Where(o => o.Supplier == supplier)
                    .CountAsync();
            return obj;
        }

        [HttpGet, Route("ProductCountCategory")]
        public async Task<int> GetProductCountByCategory([FromQuery] string category)
        {
            var obj = await _db.MasterProducts
                    .Where(o => o.Category1.Name.ToUpper() == category.ToUpper())
                    .CountAsync();
            return obj;
        }


        [HttpGet, Route("ProductsBySupplier")]
        public async Task<List<SIMProductDto>> GetProductsBySupplier([FromQuery] DataSupplier supplier)
        {
            var lst = new List<SIMProductDto>();

            var obj = await _db.MasterProducts
                .AsNoTracking()
                .Where(o => o.Supplier == supplier)
                .ToListAsync();

            if (obj != null)
            {
                foreach (var item in obj)
                {
                    var res = _mapper.Map<MasterProduct, SIMProductDto>(item);
                    lst.Add(res);
                }
            }

            return lst;
        }

        [HttpGet, Route("ProductsByCategory")]
        public async Task<List<SIMProductDto>> GetProductsByCategory([FromQuery] string category)
        {
            var lst = new List<SIMProductDto>();

            var obj = await _db.MasterProducts
                .AsNoTracking()
                .Where(o => o.Category1.Name.ToUpper() == category.ToUpper())
                .ToListAsync();

            if (obj != null)
            {
                foreach (var item in obj)
                {
                    var res = _mapper.Map<MasterProduct, SIMProductDto>(item);
                    lst.Add(res);
                }
            }

            return lst;
        }







        //[HttpPost]
        //[Route("ImportEposData")]
        //public async Task<IActionResult> ImportEposFile(IFormFile file)
        //{

        //    if (file == null || file.Length == 0)
        //        return BadRequest("No file uploaded.");



        //    return Ok();
        //}
    }
}
