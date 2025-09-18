
using Boost.Retail.Data.DTO;
using Boost.Retail.Domain.Enums;
using Boost.Retail.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BoostRetailAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class SalesController : ControllerBase
    {
        private readonly ISaleService _saleService;

        public SalesController(ISaleService saleService)
        {
            _saleService = saleService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateSale([FromBody] SaleRequest request)
        {
            var (success, message, result) = await _saleService.CreateSaleAsync(request);
            if (!success)
            {
                return BadRequest(message);
            }
            return Ok(result);
        }

        [HttpPost("return")]
        public async Task<IActionResult> ProcessReturn([FromBody] ReturnRequest request)
        {
            var (success, message, result) = await _saleService.ProcessReturnAsync(request);
            if (!success)
            {
                return BadRequest(message);
            }
            return Ok(result);
        }

        [HttpPost("complete-payment")]
        public async Task<IActionResult> CompletePayment([FromBody] CompletePaymentRequest request)
        {
            var (success, message, result) = await _saleService.CompletePaymentAsync(request);
            if (!success)
            {
                return BadRequest(message);
            }
            return Ok(result);
        }

        [HttpGet("{saleTransactionRef}")]
        public async Task<IActionResult> GetSaleTransaction(string saleTransactionRef)
        {
            var (success, message, result) = await _saleService.GetSaleTransactionAsync(saleTransactionRef);
            if (!success)
            {
                return NotFound(message);
            }
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetSaleTransactions(
            [FromQuery] string? tillId = null,
            [FromQuery] TransactionStatus? status = null,
            [FromQuery] DateTime? startDate = null,
            [FromQuery] DateTime? endDate = null,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10)
        {
            var (success, message, result) = await _saleService.GetSaleTransactionsAsync(tillId, status, startDate, endDate, page, pageSize);
            if (!success)
            {
                return BadRequest(message);
            }
            return Ok(result);
        }
       
    }
}
