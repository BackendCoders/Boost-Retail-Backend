using Boost.Retail.Data.Models;
using Boost.Retail.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BoostRetailAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public class ProductCategoryController : BaseController<ProductCategory, int>
    {
        public ProductCategoryController(IGenericService<ProductCategory, int> service) : base(service)
        {
           
        }
    }
}
