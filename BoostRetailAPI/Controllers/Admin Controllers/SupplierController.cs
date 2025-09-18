using AutoMapper;
using Boost.Admin.Logic.Interface;
using Microsoft.AspNetCore.Mvc;

namespace BoostRetailAPI.Controllers.Admin_Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SupplierController : Controller
    {
        private IMapper _mapper;
        private readonly ISupplierLogic _logic;
        public SupplierController(ISupplierLogic logic, IMapper mapper)
        {
            _logic = logic;
            _mapper = mapper;
        }

    }
}
