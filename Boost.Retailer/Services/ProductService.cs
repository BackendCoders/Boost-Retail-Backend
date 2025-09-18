
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Boost.Retail.Data;
using Boost.Retail.Data.DTO;
using Boost.Retail.Data.Models;
using Boost.Retail.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using System.Reflection;

namespace Boost.Retail.Services
{
    public class ProductService : GenericService<Product, int>, IProductService
    {
        private readonly BoostDbContext _context;
        private readonly ILogger<Product> _logger;
        private readonly IMapper _mapper;

        public ProductService(ITenantDbContextFactory contextFactory, ILogger<Product> logger, IMapper mapper) : base(() => contextFactory.Create(), logger)
        {
            _context = contextFactory.Create();
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<Product?> GetByPartNumberAsync(string partNumber)
        {
            return await _context.Products.FirstOrDefaultAsync(p => p.PartNumber == partNumber);
        }
        public async Task<Product?> GetByMFRPartNumberAsync(string mpn)
        {
            return await _context.Products.FirstOrDefaultAsync(p => p.MfrPartNumber == mpn);
        }
        public async Task<Product?> GetByBarcodeAsync(string barcode)
        {
            return await _context.Products.FirstOrDefaultAsync(p => p.Barcode == barcode);
        }

        public async Task<bool> PartNumberExistsAsync(string partNumber)
        {
            return await _context.Products.AnyAsync(p => p.PartNumber == partNumber);
        }

        public async Task<IEnumerable<ProductDto>> SearchProductsAsync(Dictionary<string, string> filters)
        {
            IQueryable<Product> query = _context.Products.AsQueryable();

            foreach (var filter in filters)
            {
                var propertyName = filter.Key;
                var value = filter.Value;

                var parameter = Expression.Parameter(typeof(Product), "p");
                var property = Expression.PropertyOrField(parameter, propertyName);

                var constant = Expression.Constant(Convert.ChangeType(value, property.Type));
                Expression predicate;

                if (property.Type == typeof(string))
                {
                    // For strings, use .Contains for partial match
                    var containsMethod = typeof(string).GetMethod("Contains", new[] { typeof(string) });
                    predicate = Expression.Call(property, containsMethod, constant);
                }
                else
                {
                    // For other types, use equality
                    predicate = Expression.Equal(property, constant);
                }

                var lambda = Expression.Lambda<Func<Product, bool>>(predicate, parameter);
                query = query.Where(lambda);
            }

            var result = await query
            .ProjectTo<ProductDto>(_mapper.ConfigurationProvider)
            .ToListAsync();

            return result.Any() ? result : null;
        }

        public async Task<IEnumerable<ProductDto>> DynamicSearchProductsAsync(string sqlQuery)
        {
            IQueryable<Product> query = _context.Products.AsQueryable();

            if (!string.IsNullOrWhiteSpace(sqlQuery))
            {
                try
                {
                    query = query.Where(sqlQuery); // Dynamic LINQ filter
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error executing dynamic search query: {Query}", sqlQuery);
                    return new List<ProductDto>(); // Return empty list on error
                }
            }

            var result = await query
          .ProjectTo<ProductDto>(_mapper.ConfigurationProvider)
          .ToListAsync();

            return result;
        }

        public async Task<IEnumerable<object>> DynamicSearchProductsAsync(string sqlQuery, params string[] productProperties)
        {
            IQueryable<Product> query = _context.Products.AsQueryable();

            if (!string.IsNullOrWhiteSpace(sqlQuery))
            {
                try
                {
                    query = query.Where(sqlQuery); // Dynamic LINQ filter
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error executing dynamic search query: {Query}", sqlQuery);
                    return new List<object>(); // Return empty list on error
                }
            }

            // If no properties specified, return all properties in ProductDto
            if (productProperties == null || !productProperties.Any())
            {
                var fullResult = await query
                    .ProjectTo<ProductDto>(_mapper.ConfigurationProvider)
                    .ToListAsync();
                return fullResult;
            }

            // Create a dynamic projection to Dictionary<string, object>
            var parameter = Expression.Parameter(typeof(Product), "p");
            var selectExpressions = new List<(string, Expression)>();

            foreach (var propertyName in productProperties)
            {
                // Ensure the property exists on Product
                var propertyInfo = typeof(Product).GetProperty(propertyName, BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
                if (propertyInfo != null)
                {
                    // Store the property name and its value expression
                    var valueExpression = Expression.Convert(Expression.Property(parameter, propertyInfo), typeof(object));
                    selectExpressions.Add((propertyName, valueExpression));
                }
                else
                {
                    _logger.LogWarning("Property {PropertyName} not found on Product entity", propertyName);
                }
            }

            if (!selectExpressions.Any())
            {
                _logger.LogError("No valid properties selected for projection");
                return new List<object>();
            }

            // Create an expression to initialize a Dictionary<string, object>
            var dictionaryType = typeof(Dictionary<string, object>);
            var addMethod = dictionaryType.GetMethod("Add", BindingFlags.Public | BindingFlags.Instance, null, new[] { typeof(string), typeof(object) }, null);
            if (addMethod == null)
            {
                _logger.LogError("Failed to find Dictionary<string, object>.Add method");
                throw new InvalidOperationException("Could not find Dictionary.Add method");
            }

            var dictionaryInit = Expression.ListInit(
                Expression.New(dictionaryType),
                selectExpressions.Select(pair => Expression.ElementInit(
                    addMethod,
                    Expression.Constant(pair.Item1), // Key
                    pair.Item2                      // Value
                ))
            );

            var lambda = Expression.Lambda<Func<Product, Dictionary<string, object>>>(dictionaryInit, parameter);

            var result = await query
                .Select(lambda)
                .ToListAsync();

            return result;
        }


        public async Task<string> AddNewPartAsync(Product item)
        {
            if (!string.IsNullOrEmpty(item.PartNumber))
            {
                throw new InvalidOperationException($"Product already contains part number '{item.PartNumber}'.");
            }

            if (!string.IsNullOrEmpty(item.MfrPartNumber))
            {
                if (await _context.Products.AnyAsync(p => p.MfrPartNumber == item.MfrPartNumber))
                    throw new InvalidOperationException($"MfrPartNumber '{item.MfrPartNumber}' already exists.");
            }

            if (!string.IsNullOrEmpty(item.Barcode))
            {
                if (await _context.Products.AnyAsync(p => p.Barcode == item.Barcode))
                    throw new InvalidOperationException($"Barcode '{item.Barcode}' already exists.");
            }

            if (string.IsNullOrEmpty(item.Search1) && string.IsNullOrEmpty(item.Search2) && string.IsNullOrEmpty(item.Details))
            {
                throw new InvalidOperationException($"Search1, Search2 and Details all can not empty.");
            }

            var partnumber = await GeneratePartNumber();
            item.PartNumber = partnumber;
            item.CreatedAt = DateTime.UtcNow;
            item.UpdatedAt = DateTime.UtcNow;

            var added = await AddAsync(item);
            return added.PartNumber;
        }

        public async Task<int> UpdatePartAsync(Product item)
        {
            try
            {
                if (string.IsNullOrEmpty(item.PartNumber))
                {
                    return 0;
                }
                else
                {
                    var product = await _context.Products.FirstOrDefaultAsync(o=> o.PartNumber == item.PartNumber);
                    if (product != null)
                    {
                        _context.Entry(product).CurrentValues.SetValues(item);
                        var val = await _context.SaveChangesAsync();
                        return val;
                    }
                    else
                    {  return 0; }  
                }
            }
            catch (KeyNotFoundException)
            {
                return 0;
            }
        }

        private async Task<string> GetLastPartNumberAsync()
        {
            var product = await _context.Products
            .OrderByDescending(p => p.PartNumber)
            .Select(p => new { p.PartNumber })
            .FirstOrDefaultAsync();

            return product?.PartNumber ?? string.Empty;
        }

        private async Task<string> GeneratePartNumber()
        {
            // Try to get the last used part number from the service
            string lastPartNumber = await GetLastPartNumberAsync();

            string nextPartNumber;
            if (string.IsNullOrEmpty(lastPartNumber))
            {
                // Start with 00001 if no previous part number exists
                nextPartNumber = "00001";
            }
            else
            {
                nextPartNumber = GetNextPartNumber(lastPartNumber);
            }

            // Ensure uniqueness
            while (await PartNumberExistsAsync(nextPartNumber))
            {
                nextPartNumber = GetNextPartNumber(nextPartNumber);
                // Handle overflow (after Z9999)
                if (nextPartNumber == "00001")
                {
                    throw new InvalidOperationException("Part number capacity exhausted.");
                }
            }

            return nextPartNumber;
        }

        private string GetNextPartNumber(string currentPartNumber)
        {
            if (currentPartNumber == "99999")
            {
                // Transition from 99999 to A0001
                return "A0001";
            }

            if (currentPartNumber.Length == 5 && currentPartNumber[0] >= 'A' && currentPartNumber[0] <= 'Z')
            {
                char prefix = currentPartNumber[0];
                int number = int.Parse(currentPartNumber.Substring(1));

                if (number < 9999)
                {
                    // Increment the number part (e.g., A0001 -> A0002)
                    return $"{prefix}{number + 1:D4}";
                }
                else if (prefix < 'Z')
                {
                    // Move to next letter (e.g., A9999 -> B0001)
                    return $"{(char)(prefix + 1)}0001";
                }
                else
                {
                    // After Z9999, loop back to 00001 (or throw an exception if capacity is exhausted)
                    return "00001";
                }
            }

            // Increment numeric part number (e.g., 00001 -> 00002)
            int currentNumber = int.Parse(currentPartNumber);
            return (currentNumber + 1).ToString("D5");
        }

        
    }
}
