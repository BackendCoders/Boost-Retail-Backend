using Boost.Retail.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BoostRetailAPI.Controllers
{
    public class BaseController<T, TId> : ControllerBase where T : class
    {
        private readonly IGenericService<T, TId> _service;

        public BaseController(IGenericService<T, TId> service)
        {
            _service = service;
        }

        [HttpGet("{id}")]
        public virtual async Task<ActionResult<T>> GetById(TId id)
        {
            var item = await _service.GetByIdAsync(id);
            if (item == null) return NotFound();
            return item;
        }

        [HttpGet]
        public virtual async Task<ActionResult<IEnumerable<T>>> GetAll()
        {
            return Ok(await _service.GetAllAsync());
        }

        [HttpPost]
        public virtual async Task<ActionResult<T>> Add([FromBody] T item)
        {
            var created = await _service.AddAsync(item);
            return Ok(created); // You may customize CreatedAtAction if you want
        }

        [HttpPut("{id}")]
        public virtual async Task<IActionResult> Update(TId id, [FromBody] T updatedItem)
        {
            try
            {
                await _service.UpdateAsync(id, updatedItem);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpDelete("{id}")]
        public virtual async Task<IActionResult> Delete(TId id)
        {
            try
            {
                await _service.DeleteAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<T>>> SearchProducts([FromQuery] Dictionary<string, string> filters)
        {
            return _service.SearchProductsAsync(filters) switch
            {
                var items when items != null => Ok(await items),
                _ => NotFound()
            };
        }

        [HttpGet("dynamicsearch")]
        public async Task<ActionResult<IEnumerable<T>>> DynamicSearchProducts([FromQuery] string filter)
        {
            return _service.DynamicSearchProductsAsync(filter) switch
            {
                var items when items != null => Ok(await items),
                _ => NotFound()
            };
        }
    }

}
