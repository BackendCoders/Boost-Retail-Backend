using AutoMapper;
using Boost.Admin.Data;
using Boost.Admin.Data.Models;
using Boost.Admin.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Serilog;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.Json;
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
            var existingItem = await _db.CategoryLookups
                .FirstOrDefaultAsync(o => o.TableName.ToLower() == item.TableName.ToLower()
                                       && o.SupplierFeed == item.SupplierFeed);

            if (existingItem == null)
            {
                // Create new CategoryLookup
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

                // Insert one blank row in CategoryMaps with the new TableId
                var newCategoryMap = new CategoryMap()
                {
                    Brand = 0,
                    BrandName = string.Empty,
                    Model = string.Empty,
                    Category1 = string.Empty,
                    Category2 = string.Empty,
                    Category3 = string.Empty,
                    TableId = newCategory.Id,
                    Filters = null
                };

                _db.CategoryMaps.Add(newCategoryMap);
                await _db.SaveChangesAsync();

                return newCategory.Id;
            }
            else
            {
                // Update existing CategoryLookup
                existingItem.TableName = item.TableName;
                existingItem.SupplierFeed = item.SupplierFeed;
                existingItem.Categorisation = item.Categorisation;
                existingItem.SupplierColumns = item.SupplierColumns;
                existingItem.IsActive = item.IsActive;

                _db.CategoryLookups.Update(existingItem);
                await _db.SaveChangesAsync();

                return existingItem.Id;
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

            // delete all related CategoryMaps first
            var relatedMaps = _db.CategoryMaps.Where(m => m.TableId == id);
            _db.CategoryMaps.RemoveRange(relatedMaps);

            _db.CategoryLookups.Remove(item);
            var res = await _db.SaveChangesAsync();
            return res;
        }
        public async Task<int> DeleteCategoryMapAsync(int id)
        {
            var item = await _db.CategoryMaps.FindAsync(id);
            if (item == null)
                throw new KeyNotFoundException($"CategoryMap with Id {id} not found");

            _db.CategoryMaps.Remove(item);
            var res = await _db.SaveChangesAsync();
            return res;
        }


        


        /// <summary>
        /// Code By Ashwani
        /// </summary>
        /// <param name="lookupId"></param>
        /// <returns></returns>

   

        public async Task<List<CategoryMapRequestdto>> GetCategoryMaps(int lookupId)
        {
            var resultList = new List<CategoryMapRequestdto>();
            var categoryMapData = _db.CategoryMaps.Where(c => c.TableId == lookupId);

            foreach (var cat in categoryMapData)
            {
                var returnObj = new CategoryMapRequestdto();
                var list = new List<DynamicPropertyDto>();

                var lookup = await _db.CategoryLookups.FirstOrDefaultAsync(o => o.Id == lookupId);
                if (lookup != null)
                {
                    var filterColumnsField = lookup.SupplierColumns;
                    var filterColumns = filterColumnsField.Split(',');

                    foreach (var col in filterColumns)
                    {
                        list.Add(new DynamicPropertyDto()
                        {
                            ColumnName = col.Trim(),
                            Filter = string.Empty,
                            Value = string.Empty,
                        });
                    }

                    if (lookup.Categorisation == "Categories")
                    {
                        var list2 = new List<DynamicPropertyDto>()
                        {
                            new DynamicPropertyDto { ColumnName = "Category 1", Filter = string.Empty, Value = string.Empty, ColumnType = 1 },
                            new DynamicPropertyDto { ColumnName = "Category 2", Filter = string.Empty, Value = string.Empty, ColumnType = 1 },
                            new DynamicPropertyDto { ColumnName = "Category 3", Filter = string.Empty, Value = string.Empty, ColumnType = 1 }
                        };
                        list.AddRange(list2);
                    }
                    else if (lookup.Categorisation == "Search 1/2")
                    {
                        var list2 = new List<DynamicPropertyDto>()
                        {
                            new DynamicPropertyDto { ColumnName = "Model", Filter = string.Empty, Value = string.Empty },
                            new DynamicPropertyDto { ColumnName = "Brand Name", Filter = string.Empty, Value = string.Empty }
                        };
                        list.AddRange(list2);
                    }

                    list.Add(new DynamicPropertyDto()
                    {
                        ColumnName = "Is Active",
                        Filter = string.Empty,
                        Value = lookup.IsActive,
                        ColumnType = 2
                    });

                    returnObj.Id = cat.Id;
                    returnObj.DynamicProperties = list;
                }

                resultList.Add(returnObj);
            }

            return resultList;
        }

        public async Task<List<string>> GetSupplierColumns(BrandType supplier)
        {
            var type = FindSupplierDtoType(supplier);
            if (type == null)
                return new List<string>();

            var props = type.GetProperties()
                            .Select(p => p.Name)
                            .ToList();

            return props;
        }
        private Type? FindSupplierDtoType(BrandType supplier)
        {
            var assembly = typeof(CategoryLogic).Assembly;

            // Look for classes under namespace SIM.Suppliers.<BrandName>
            string supplierName = supplier.ToString(); // e.g. "Cube", "Giant", "Haibike"

            var dtoType = assembly.GetTypes()
                .FirstOrDefault(t =>
                    t.Namespace != null &&
                    t.Namespace.Contains($"Suppliers.{supplierName}") &&
                    t.Name.EndsWith("Dto", StringComparison.OrdinalIgnoreCase));

            return dtoType;
        }
        public async Task<List<CategoryDto>> GetCategoryByParentId(int? parentId)
        {
            var category = await _db.Categories.Where(x => x.ParentId == parentId).ToListAsync();

            var childCategory = _mapper.Map<List<CategoryDto>>(category);

            return childCategory;
        }
        public async Task<int> SaveCategoryMapAsync(CategoryMapRequestdto request,int Tableid)
        {
            if (request == null)
                throw new KeyNotFoundException("Invalid CategoryMap request");

            var filters = new List<object>();
            foreach (var prop in request.DynamicProperties) // prop is DynamicPropertyDto
            {
               
                if (prop.Value != null)
                {
                    // Try to deserialize to FilterField if JSON-like string
                    if (prop.Value is string strVal && strVal.TrimStart().StartsWith("{"))
                    {
                        var filterField = JsonConvert.DeserializeObject<FilterField>(strVal);

                        filters.Add(new
                        {
                            ColumnName = prop.ColumnName,
                            Value = filterField.Value,
                            Filter = filterField.Filter
                        });
                    }
                    else
                    {
                        filters.Add(new
                        {
                            ColumnName = prop.ColumnName,
                            Value = prop.Value.ToString(),
                            Filter = (string)null
                        });
                    }
                }
            }

            string filtersJson = JsonConvert.SerializeObject(filters);

            var existingMap = await _db.CategoryMaps
                .FirstOrDefaultAsync(x => x.Id == request.Id);

            if (existingMap == null)
            {
                var newCategoryMap = new CategoryMap
                {
                    TableId = Tableid,
                    Filters = filtersJson
                };

                _db.CategoryMaps.Add(newCategoryMap);
                await _db.SaveChangesAsync();

                return newCategoryMap.Id;
            }
            else
            {
                existingMap.Filters = filtersJson;

                _db.CategoryMaps.Update(existingMap);
                await _db.SaveChangesAsync();

                return existingMap.Id;
            }
        }

       
    }
}
