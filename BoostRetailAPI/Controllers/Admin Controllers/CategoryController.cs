using AutoMapper;
using Boost.Admin.Data;
using Boost.Admin.Data.Models;
using Boost.Admin.DTOs;
using Boost.Admin.Logic;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace BoostRetailAPI.Controllers.Admin_Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private IMapper _mapper;
        private readonly ICategoryLogic _logic;


        public CategoryController(ICategoryLogic logic, IMapper mapper)
        {
            _logic = logic;
            _mapper = mapper;
        }




        [HttpPost]
        public async Task<IActionResult> Add([FromBody] CategoryDto item)
        {
            try
            {
                var created = await _logic.AddAsync(item);
                return Ok(created); // You may customize CreatedAtAction if you want
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] CategoryDto updatedItem)
        {
            try
            {
                var res = await _logic.UpdateAsync(id, updatedItem);
                return Ok(res);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var res = await _logic.DeleteAsync(id);
                return Ok(res);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpGet, Route("GetAllCategories")]
        public async Task<IActionResult> GetAllCategories()
        {
            var tree = await _logic.GetCategories();
            return Ok(tree);
        }


        [HttpGet, Route("GetCategoriesBySupplier")]
        public async Task<IActionResult> GetCategoriesBySupplier([FromQuery] DataSupplier supplier)
        {
            var tree = await _logic.GetCategories(supplier);
            return Ok(tree);
        }


        [HttpGet, Route("GetCategoryParents")]
        public async Task<IActionResult> GetCategoryParents(int categoryId)
        {
            var parents = await _logic.GetCategoryParents(categoryId);
            return Ok(parents);
        }


        [HttpGet, Route("GetCategoryLookups")]
        public async Task<IActionResult> GetCategoryLookups() 
        {
            var lookup = await _logic.GetCategoryLookups();
            return Ok(lookup);
        }


        [HttpPost, Route("AddCategoryLookupAsync")]
        public async Task<IActionResult> AddCategoryLookupAsync([FromBody] CategoryLookup item)
        {
            try
            {
                var created = await _logic.AddCategoryLookupAsync(item);
                return Ok(created); // You may customize CreatedAtAction if you want
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }
        

        [HttpPut("UpdateCategoryLookupAsync/{id}")]
        public async Task<IActionResult> UpdateCategoryLookupAsync(int id, [FromBody] CategoryLookup updatedItem)
        {
            try
            {
                var res = await _logic.UpdateCategoryLookupAsync(id, updatedItem);
                return Ok(res);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }


        [HttpDelete("DeleteCategoryLookupAsync/{id}")]
        public async Task<IActionResult> DeleteCategoryLookupAsync(int id)
        {
            try
            {
                var res = await _logic.DeleteCategoryLookupAsync(id);
                return Ok(res);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpGet, Route("GetCategoryMaps")]
        public async Task<IActionResult> GetCategoryMaps(int lookupId)
        {
            var cmaps = await _logic.GetCategoryMaps(lookupId);
            return Ok(cmaps);
        }


        [HttpGet, Route("GetSupplierColumns")]
        public async Task<IActionResult> GetSupplierColumns(BrandType supplier)
        {
            var scolumns = await _logic.GetSupplierColumns(supplier);
            return Ok(scolumns);
        }


        [HttpGet, Route("GetCategoryByParentId")]
        public async Task<IActionResult> GetCategoryByParentId(int? parentId)
        {
            var categories = await _logic.GetCategoryByParentId(parentId);

            return Ok(categories);
        }


              
    }
}
