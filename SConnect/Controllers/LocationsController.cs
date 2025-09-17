using BoostRetail.Integrations.SConnect.DTOs;
using BoostRetail.Integrations.SConnect.Services;
using Microsoft.AspNetCore.Mvc;

namespace SConnect.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LocationsController : Controller
    {
        private readonly LocationService _service;

        public LocationsController(LocationService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetInventory([FromQuery] PaginationQuery query)
        {
            var response = new PaginatedResponse<LocationResponseDto>
            {
                Attribute = new AttributeInfo
                {
                    Count = 0,
                    Offset = query.Offset,
                    Limit = query.Limit
                },
                Values = new List<LocationResponseDto>()
            };

            var data  = await _service.GetLocations();

            if(data.Count > 0)
            {
                response.Attribute.Count = data.Count;
                response.Values = data.Skip(query.Offset).Take(query.Limit);
            }

            return Ok(response);
        }
    }
}
