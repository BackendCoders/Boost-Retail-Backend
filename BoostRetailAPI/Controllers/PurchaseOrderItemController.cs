
using Boost.Retail.Data.Models;
using Boost.Retail.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BoostRetailAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class PurchaseOrderItemController : BaseController<PurchaseOrderItem, int>
    {
        public PurchaseOrderItemController(IGenericService<PurchaseOrderItem, int> service) : base(service)
        {

        }
    }
}
