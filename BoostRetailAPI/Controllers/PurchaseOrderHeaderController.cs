
using Boost.Retail.Data.Models;
using Boost.Retail.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BoostRetailAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class PurchaseOrderHeaderController : BaseController<PurchaseOrderHeader, int>
    {
        public PurchaseOrderHeaderController(IGenericService<PurchaseOrderHeader, int> service) : base(service)
        {

        }
    }
}
