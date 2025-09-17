
using Boost.Retail.Data.Models;
using Boost.Retail.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BoostRetailAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class ProductLevelsController : BaseController<ProductLevel, int>
    {
        public ProductLevelsController(IGenericService<ProductLevel, int> service) : base(service)
        {

        }

    }
}
