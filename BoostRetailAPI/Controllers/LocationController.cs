
using Boost.Retail.Data.Models;
using Boost.Retail.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BoostRetailAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class LocationController : BaseController<Location, int>
    {
        public LocationController(IGenericService<Location, int> service) : base(service)
        {

        }
    }
}
