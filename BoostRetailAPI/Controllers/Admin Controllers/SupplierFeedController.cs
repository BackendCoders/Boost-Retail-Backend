using AutoMapper;
using Boost.Admin.DTOs;
using Boost.Admin.Logic;
using Boost.Admin.Logic.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace BoostRetailAPI.Controllers.Admin_Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SupplierFeedController : ControllerBase
    {
        private IMapper _mapper;
        private readonly ISupplierFeedLogic _logic;

        public SupplierFeedController(ISupplierFeedLogic logic, IMapper mapper)
        {
            _logic = logic;
            _mapper = mapper;
        }


        [HttpPost]
        public async Task<IActionResult> Add([FromBody] SupplierFeedDto item)
        {
            var response = await _logic.AddAsync(item);

            return Ok(response);
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id ,  [FromBody] SupplierFeedDto item)
        {
            var response = await _logic.UpdateAsync(id, item);

            return Ok(response);
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var response = await _logic.DeleteAsync(id);
            return Ok(response);
        }


        [HttpGet]
        public async Task<IActionResult> GetAll(PaginationDto req)
        {
            var response = await _logic.GetAllSupplierFeed(req);
            return Ok(response);
        }


    }
}
