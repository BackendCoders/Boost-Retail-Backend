
using Boost.Retail.Data.DTO;
using Boost.Retail.Data.Models;
using Boost.Retail.Services.Interfaces;
using BoostRetailLib.Data.DTO;
using Microsoft.AspNetCore.Mvc;

namespace BoostRetailAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class InventoryController : BaseController<Inventory, int>
    {
        private readonly IInventoryService _inventoryService;
        private readonly IProductService _productService;
        public InventoryController(IInventoryService inventoryService, IProductService productService) : base(inventoryService)
        {
            _inventoryService = inventoryService;
            _productService = productService;
        }

        [HttpPost("GetStock")]
        public async Task<ActionResult<int>> GetStock([FromBody] GetStockRequest req)
        {
            var partexists = await _productService.PartNumberExistsAsync(req.PartNumber);
            if (partexists)
            {
                var stock = await _inventoryService.GetStockAsync(req.PartNumber, req.LocationCode);
                return Ok(stock);
            }
            else
                return NotFound($"Part number '{req.PartNumber}' does not exist in the system.");
        }

        [HttpPost("AddStock")]
        public async Task<ActionResult<int>> AddStock([FromBody] SetStockRequest req)
        {
            var partexists = await _productService.PartNumberExistsAsync(req.PartNumber);
            if (partexists)
            {
                var stock = await _inventoryService.AddStockAsync(req.PartNumber, req.LocationCode, req.Stock, req.StockNumbers);
                return Ok(stock);
            }
            else
                return NotFound($"Part number '{req.PartNumber}' does not exist in the system.");
        }
    }
}
