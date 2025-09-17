using Boost.Retail.Data.Models;
using Boost.Retail.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BoostRetailAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class CustomerController : BaseController<Customer, int>
    {
        public CustomerController(IGenericService<Customer, int> service) : base(service)
        {

        }
    }
}
