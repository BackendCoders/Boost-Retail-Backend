
using Boost.Retail.Data.DTO;
using Boost.Retail.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BoostRetailAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class TillController : ControllerBase
    {
        private readonly ITillService _tillService;
        private readonly ILogger<ProductsController> _logger;

        public TillController(ITillService tillService, ILogger<ProductsController> logger)
        {
            _tillService = tillService;
            _logger = logger;
        }

        [HttpGet("getTillProductShortcuts")]
        public async Task<ActionResult<List<TillProductShortcutDTO>>> GetTillProductShortcuts(string SetID)
        {
            var products = await _tillService.GetTillProductShortcuts(SetID);
            if (products == null || products.Count == 0)
            {
                return NotFound();
            }

            return products;
        }


        [HttpGet("getTillProduct")]
        public async Task<ActionResult<TillProduct>> GetTillProduct(string partnumber, string locCode)
        {
            var product = await _tillService.GetTillProduct(partnumber, locCode);
            if (product == null)
            {
                return NotFound();
            }

            return product;
        }

        [HttpGet("getTillCustomer")]
        public async Task<ActionResult<TillCustomer>> GetTillCustomer(string customerAcc)
        {
            var customer = await _tillService.GetTillCustomer(customerAcc);
            if (customer == null)
            {
                return NotFound();
            }

            return customer;
        }
    }
}
