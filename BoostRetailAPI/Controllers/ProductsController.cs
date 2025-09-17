
using Boost.Retail.Data.DTO;
using Boost.Retail.Data.Models;
using Boost.Retail.Services.Interfaces;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace BoostRetailAPI.Controllers
{
    // Products Controller
    [ApiController]
    [Route("api/[controller]")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class ProductsController : BaseController<Product, int>
    {
        private readonly IProductService _productService;
        private readonly ILogger<ProductsController> _logger;

        public ProductsController(IProductService productService, ILogger<ProductsController> logger) : base(productService)
        {
            _productService = productService;
            _logger = logger;
        }


        /// <summary>
        /// Hide base method - and override to provide custom implementation.
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        [NonAction]
        public override async Task<ActionResult<Product>> Add([FromBody] Product item)
        {
            var created = await _productService.AddAsync(item);
            return Ok(created); 
        }

        [NonAction]
        public override async Task<IActionResult> Update(int id, [FromBody] Product updatedItem) 
        {
            return NoContent();
        }

        [HttpPut("{partnumber}")]
        public async Task<ActionResult<int>> UpdateProduct(string partnumber, [FromBody] Product item) 
        {
            item.PartNumber = partnumber;
            var updated = await _productService.UpdatePartAsync(item);
            return Ok(updated);
        }

        [HttpPatch("{partnumber}")]
        public async Task<IActionResult> PatchProduct(string partnumber, [FromBody] JsonPatchDocument<Product> patchDoc)
        {
            if (patchDoc == null)
                return BadRequest();

            var product = await _productService.GetByPartNumberAsync(partnumber);
            if (product == null)
                return NotFound();

            patchDoc.ApplyTo(product, (Microsoft.AspNetCore.JsonPatch.Adapters.IObjectAdapter)ModelState); // apply the patch to the retrieved object

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var updated = await _productService.UpdatePartAsync(product);
            return Ok(updated);
        }

        [HttpPost]
        public async Task<ActionResult<string>> AddNew([FromBody] Product item)
        {
            try
            {
                var created = await _productService.AddNewPartAsync(item);
                return Ok(created);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding new product: {Message}", ex.Message);
                return BadRequest($"Failed to add new product. {ex.Message}");
            }
        }


        [HttpGet("partnumber/{partnumber}")]
        public async Task<ActionResult<Product>> GetByPartNumber(string partnumber)
        {
            var product = await _productService.GetByPartNumberAsync(partnumber);
            if (product == null)
            {
                _logger.LogInformation($"part {partnumber} not found.");
                return NotFound();
            }

            return product;
        }

        [HttpGet("mfrpartnumber/{mfrpartnumber}")]
        public async Task<ActionResult<Product>> GetByMFRPartNumber(string mfrpartnumber)
        {
            var product = await _productService.GetByMFRPartNumberAsync(mfrpartnumber);
            if (product == null)
                return NotFound();

            return product;
        }


        [HttpGet("barcode/{barcode}")]
        public async Task<ActionResult<Product>> GetByBarcode(string barcode)
        {
            var product = await _productService.GetByBarcodeAsync(barcode);
            if (product == null)
                return NotFound();

            return product;
        }

        [HttpGet("search_dto")]
        public new async Task<ActionResult<IEnumerable<ProductDto>>> SearchProducts([FromQuery] Dictionary<string, string> filters)
        {
            return _productService.SearchProductsAsync(filters) switch
            {
                var items when items != null => Ok(await items),
                _ => NotFound()
            };
        }

        [HttpGet("dynamicsearch_dto")]
        public new async Task<ActionResult<IEnumerable<ProductDto>>> DynamicSearchProducts([FromQuery] string filter)
        {
            return _productService.DynamicSearchProductsAsync(filter) switch
            {
                var items when items != null => Ok(await items),
                _ => NotFound()
            };
        }

        [HttpGet("dynamicsearch_customProperties")]
        public new async Task<ActionResult<IEnumerable<object>>> DynamicSearchProducts([FromQuery] string filter, [FromQuery(Name = "properties")] string properties)
        {
            var productProperties = string.IsNullOrWhiteSpace(properties)
        ? Array.Empty<string>()
        : properties.Split(',', StringSplitOptions.RemoveEmptyEntries)
                    .Select(p => p.Trim())
                    .ToArray();
            return _productService.DynamicSearchProductsAsync(filter, productProperties) switch
            {
                var items when items != null => Ok(await items),
                _ => NotFound()
            };
        }

    }

}
