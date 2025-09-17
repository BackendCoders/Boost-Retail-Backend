using Boost.Retail.Data.Models;
using Boost.Retail.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BoostRetailAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class InventoryEntryRecordController : BaseController<InventoryEntryRecord, int>
    {
        public InventoryEntryRecordController(IGenericService<InventoryEntryRecord, int> service) : base(service)
        {

        }

    }
}
