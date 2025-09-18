using BoostRetail.Integrations.SConnect.DTOs;
using BoostRetail.Integrations.SConnect.Services;
using Microsoft.AspNetCore.Mvc;

namespace SConnect.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TransactionsController : Controller
    {
        private readonly TransactionsService _service;

        public TransactionsController(TransactionsService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetTransactions([FromQuery] PaginationQuery query)
        {
            var response = new PaginatedResponse<TransactionResponseDto>
            {
                Attribute = new AttributeInfo
                {
                    Count = 0,
                    Offset = query.Offset,
                    Limit = query.Limit
                },
                Values = new List<TransactionResponseDto>()
            };

            var data = await _service.GetTransactions();

            if (data.Count > 0)
            {
                response.Attribute.Count = data.Count;
                response.Values = data.Skip(query.Offset).Take(query.Limit);
            }

            return Ok(response);
        }
    }
}
