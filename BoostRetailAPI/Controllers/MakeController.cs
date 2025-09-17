
using Boost.Retail.Data.Models;
using Boost.Retail.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BoostRetailAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class MakeController : BaseController<Make, int>
    {
        public MakeController(IGenericService<Make, int> service) : base(service)
        {

        }
    }
}
