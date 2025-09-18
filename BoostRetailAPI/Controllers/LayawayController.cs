
using Boost.Retail.Data.Models;
using Boost.Retail.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Boost.Retail.Data.Models;

namespace BoostRetailAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class LayawayController : ControllerBase
    {
        private readonly ILayawayService _service;
        public LayawayController(ILayawayService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Layaway>>> GetAllLayaway(string customerAccount)
        {
            return Ok(await _service.GetAllLayawayAsync(customerAccount));
        }

        [HttpPost]
        public async Task<ActionResult<bool>> AddLayawayAsync(Layaway layaway)
        {
            return Ok(await _service.AddLayawayAsync(layaway));
        }

        [HttpPut("{layawayId}/newquantity")]
        public async Task<ActionResult<bool>> UpdateLayawayQuantityAsync(int layawayId, int newquantity)
        {
            return Ok(await _service.UpdateLayawayQuantityAsync(layawayId, newquantity));
        }

        [HttpDelete("{layawayId}")]
        public async Task<ActionResult<bool>> DeleteLayawayAsync(int layawayId)
        {
            return Ok(await _service.DeleteLayawayAsync(layawayId));
        }
    }
}
