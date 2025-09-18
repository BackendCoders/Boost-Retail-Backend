
using Boost.Retail.Data.Models;
using Boost.Retail.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BoostRetailAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [ApiExplorerSettings(IgnoreApi = true)]

    public class TillPaymentTypesController : BaseController<TillPaymentType, int>
    {
        public TillPaymentTypesController(IGenericService<TillPaymentType, int> service) : base(service)
        {

        }
    }
}
