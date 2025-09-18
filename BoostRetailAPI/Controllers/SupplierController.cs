
using Boost.Retail.Data.Models;
using Boost.Retail.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BoostRetailAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class SupplierController : BaseController<Supplier, int>
    {
        public SupplierController(IGenericService<Supplier, int> service) : base(service)
        {

        }
    }
}
