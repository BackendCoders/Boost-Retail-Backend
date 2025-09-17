using AutoMapper;
using Boost.Admin.Data;
using Boost.Admin.Data.Models;
using Boost.Admin.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.Query.Internal.ExpressionTreeFuncletizer;

namespace Boost.Admin.Logic.Implementiation
{
    public class CategoryLogic : ICategoryLogic
    {
        private readonly ILogger _logger;
        private readonly SimDbContext _db;
        private readonly IMapper _mapper;
        
        
        public CategoryLogic(SimDbContext db, IMapper mapper)
        {
            _db = db;
            _logger = Log.ForContext<CategoryLogic>();
            _mapper = mapper;
        }



        public async Task<Category> AddAsync(CategoryDto item)
        {
            if (item.ParentId == 0)
            {
                item.ParentId = null; // Set ParentId to null if it's 0, indicating a top-level category
            }

            var existingItem = await _db.Categories.FirstOrDefaultAsync(o=> o.Name.ToLower() == item.Name.ToLower() && o.ParentId == item.ParentId);
            if (existingItem == null)
            {
                var newCategory = new Category()
                {
                    Name = item.Name,
                    ParentId = item.ParentId,
                };
                _db.Categories.Add(newCategory);
                await _db.SaveChangesAsync();
                return newCategory;
            }
            else
            {
                _logger.Warning("Category with name {Name} already exists", item.Name);
                throw new InvalidOperationException($"Category with name {item.Name} already exists.");
            }
        }

        public async Task<int> UpdateAsync(int id, CategoryDto updatedItem)
        {
            var existingItem = await _db.Categories.FindAsync(id);
            if (existingItem == null) throw new KeyNotFoundException();

            _db.Entry(existingItem).CurrentValues.SetValues(updatedItem);
            var res = await _db.SaveChangesAsync();
            return res;
        }

        public async Task<int> DeleteAsync(int id)
        {
            var existingParent = await _db.Categories.FirstOrDefaultAsync(o => o.ParentId == id);
            if (existingParent != null)
            {
                _logger.Warning("Cannot delete category with id {Id} because it has subcategories", id);
                throw new InvalidOperationException($"Cannot delete category with id {id} because it has subcategories.");
            }

            var item = await _db.Categories.FindAsync(id);
            if (item == null) throw new KeyNotFoundException();

            _db.Categories.Remove(item);
            var res = await _db.SaveChangesAsync();
            return res;
        }

        public async Task<List<CategoryNode>> GetCategories()
        {
            var category1Nodes = new List<CategoryNode>();

            var categories = await _db.Categories
                .Include(o => o.SubCategories)
                .Include(o => o.Cat1Products)
                .Include(o => o.Cat2Products)
                .Include(o => o.Cat3Products)
                .ToListAsync();
          
            foreach (var cata in categories) 
            {
                if (cata.ParentId == null)
                {
                    var productsACount = cata.Cat1Products.Count();
                    var catANode = new CategoryNode
                    {
                        Name = cata.Name ?? "Unknown Category 1",
                        ParentId = cata.ParentId,
                        Count = productsACount,
                        Id = cata.Id,
                        Children = new List<CategoryNode>(),
                    };
                    foreach (var catb in cata.SubCategories)
                    {
                        var productsBCount = catb.Cat2Products.Count();
                        var catBNode = new CategoryNode
                        {
                            Name = catb.Name ?? "Unknown Category 2",
                            ParentId = catb.ParentId,
                            Count = productsBCount,
                            Id = catb.Id,
                            Children = new List<CategoryNode>(),
                        };
                        catANode.Children.Add(catBNode);

                        foreach (var catc in catb.SubCategories)
                        {
                            var productsCCount = catc.Cat3Products.Count();
                            var catCNode = new CategoryNode
                            {
                                Name = catc.Name ?? "Unknown Category 3",
                                ParentId = catc.ParentId,
                                Count = productsCCount,
                                Id = catc.Id,
                            };
                            catBNode.Children.Add(catCNode);
                        }
                    }
                    category1Nodes.Add(catANode);
                }
            }

            return category1Nodes;
        }
      
        public async Task<List<CategoryNode>> GetCategories(DataSupplier supplier)
        {
            var category1Nodes = new List<CategoryNode>();
            var products = await _db.MasterProducts
           .Include(o => o.Category1)
           .Include(o => o.Category2)
           .Include(o => o.Category3)
           .Include(o => o.Brand)
           .Where(o => o.Category1Id != null && o.Supplier == supplier)
           .ToListAsync();

            var groupedByCat1 = products.GroupBy(p => p.Category1?.Name).OrderBy(g => g.Key);

            foreach (var catA in groupedByCat1)
            {
                var catANode = new CategoryNode
                {
                    Name = catA.Key ?? "Unknown Category 1",
                    Count = catA.Count(),
                    Id = catA.FirstOrDefault()?.Category1Id,
                    ParentId = null,
                    Children = new List<CategoryNode>()
                };
                category1Nodes.Add(catANode);

                var groupedByCat2 = catA.GroupBy(p => p.Category2?.Name).OrderBy(g => g.Key);

                foreach (var catB in groupedByCat2)
                {
                    var catBNode = new CategoryNode
                    {
                        Name = catB.Key ?? "Unknown Category 2",
                        Count = catB.Count(),
                        Id = catB.FirstOrDefault()?.Category2Id,
                        ParentId = catANode.Id,
                        Children = new List<CategoryNode>(),
                    };
                    catANode.Children.Add(catBNode);

                    var groupedByCat3 = catB.GroupBy(p => p.Category3?.Name).OrderBy(g => g.Key);

                    foreach (var catC in groupedByCat3)
                    {
                        var catCNode = new CategoryNode
                        {
                            Name = catC.Key ?? "Unknown Category 3",
                            ParentId = catBNode.Id,
                            Count = catC.Count(),
                            Id = catC.FirstOrDefault()?.Category3Id,
                            Children = new List<CategoryNode>(),
                        };
                        catBNode.Children.Add(catCNode);
                    }
                }
            }

            return category1Nodes;
        }

        public async Task<List<CategoryDto>> GetCategoryParents(int categoryId)
        {
            var categoryParents = new List<CategoryDto>();

            var category = await _db.Categories.FirstOrDefaultAsync(c => c.Id == categoryId);
            if (category != null) 
            {
                var parent = await _db.Categories.FirstOrDefaultAsync(c => c.Id == category.ParentId);
                if (parent != null)
                {
                    var categories = await _db.Categories
                       .Where(o => o.ParentId == parent.ParentId)
                       .ToListAsync();
                    foreach (var c in categories) 
                    {
                        categoryParents.Add(new CategoryDto()
                        {
                            Id = c.Id,
                            ParentId = c.ParentId,
                            Name = c.Name,
                        });
                    }
                }
            }
            return categoryParents;
        }

        public async Task<List<CategoryLookup>> GetCategoryLookups()
        {
            var categories = await _db.CategoryLookups.ToListAsync();
            return categories;
        }

        public async Task<int> AddCategoryLookupAsync(CategoryLookup item)
        {
            var existingItem = await _db.CategoryLookups.FirstOrDefaultAsync(o => o.TableName.ToLower() == item.TableName.ToLower() && o.SupplierFeed == item.SupplierFeed);
            if (existingItem == null)
            {
                var newCategory = new CategoryLookup()
                {
                    TableName = item.TableName,
                    SupplierFeed = item.SupplierFeed,
                    Categorisation = item.Categorisation,
                    IsActive = true,
                    SupplierColumns = item.SupplierColumns
                };
                _db.CategoryLookups.Add(newCategory);
                await _db.SaveChangesAsync();
                return newCategory.Id;
            }
            else
            {
                _logger.Warning($"Category lookup with name {item.TableName} already exists");
                throw new InvalidOperationException($"Category lookup with name {item.TableName} already exists");
            }
        }

        public async Task<int> UpdateCategoryLookupAsync(int id, CategoryLookup updatedItem)
        {
            var existingItem = await _db.CategoryLookups.FindAsync(id);
            if (existingItem == null) throw new KeyNotFoundException();

            _db.Entry(existingItem).CurrentValues.SetValues(updatedItem);
            var res = await _db.SaveChangesAsync();
            return res;
        }

        public async Task<int> DeleteCategoryLookupAsync(int id)
        {
            var item = await _db.CategoryLookups.FindAsync(id);
            if (item == null) throw new KeyNotFoundException();

            _db.CategoryLookups.Remove(item);
            var res = await _db.SaveChangesAsync();
            return res;
        }

        public async Task<List<CategoryMap>> GetCategoryMaps(int lookupId)
        {
            var lookup = await _db.CategoryLookups.FindAsync(lookupId);
            if (lookup == null) throw new KeyNotFoundException();

            var categorymaps = await _db.CategoryMaps.Where(o=> o.Brand == lookup.SupplierFeed).ToListAsync();
            return categorymaps;
        }

        public async Task<List<string>> GetSupplierColumns(BrandType supplier)
        {
            return new List<string> { "Title", "Model", "Brand", "Department" };
        }
    


        public async Task<List<CategoryDto>> GetCategoryByParentId(int? parentId)
        {
            var category = await _db.Categories.Where( x => x.ParentId == parentId).ToListAsync();

            var childCategory = _mapper.Map<List<CategoryDto>>(category);

            return childCategory;
        }


    }
}
