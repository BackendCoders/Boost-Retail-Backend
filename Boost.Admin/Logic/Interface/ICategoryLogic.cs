using Boost.Admin.Data;
using Boost.Admin.Data.Models;
using Boost.Admin.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Boost.Admin.Logic
{
    public interface ICategoryLogic
    {
        Task<List<CategoryNode>> GetCategories();
        Task<List<CategoryNode>> GetCategories(DataSupplier supplier);
        Task<List<CategoryDto>> GetCategoryParents(int categoryId);

        Task<Category> AddAsync(CategoryDto item);
        Task<int> UpdateAsync(int id, CategoryDto updatedItem);
        Task<int> DeleteAsync(int id);


       
        Task<List<CategoryLookup>> GetCategoryLookups();
        Task<int> AddCategoryLookupAsync(CategoryLookup item);
        Task<int> UpdateCategoryLookupAsync(int id, CategoryLookup updatedItem);
        Task<int> DeleteCategoryLookupAsync(int id);
        Task<List<string>> GetSupplierColumns(BrandType supplier);
        Task<List<CategoryMap>> GetCategoryMaps(int lookupId);


        Task<List<CategoryDto>> GetCategoryByParentId(int? parentId);
    }
}
