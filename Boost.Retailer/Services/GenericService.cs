using Boost.Retail.Data;
using Boost.Retail.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;

namespace Boost.Retail.Services
{
    public class GenericService<T, TId> : IGenericService<T, TId> where T : class
    {
        protected readonly BoostDbContext _context;
        protected readonly DbSet<T> _dbSet;
        private readonly ILogger<T> _logger;

        public GenericService(Func<BoostDbContext> contextFactory, ILogger<T> logger)
        {
            _context = contextFactory();
            _dbSet = _context.Set<T>();
            _logger = logger;
        }

        public async Task<IEnumerable<T>> GetAllAsync() => await _dbSet.ToListAsync();

        public async Task<T> GetByIdAsync(TId id) => await _dbSet.FindAsync(id);

        public async Task<T> AddAsync(T item)
        {
            _dbSet.Add(item);
            await _context.SaveChangesAsync();
            return item;
        }

        public async Task UpdateAsync(TId id, T updatedItem)
        {
            var existingItem = await _dbSet.FindAsync(id);
            if (existingItem == null) throw new KeyNotFoundException();

            _context.Entry(existingItem).CurrentValues.SetValues(updatedItem);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(TId id)
        {
            var item = await _dbSet.FindAsync(id);
            if (item == null) throw new KeyNotFoundException();

            _dbSet.Remove(item);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<T>> SearchProductsAsync(Dictionary<string, string> filters)
        {
            IQueryable<T> query = _dbSet.AsQueryable();

            foreach (var filter in filters)
            {
                var propertyName = filter.Key;
                var value = filter.Value;

                var parameter = Expression.Parameter(typeof(T), "p");
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

                var lambda = Expression.Lambda<Func<T, bool>>(predicate, parameter);
                query = query.Where(lambda);
            }

            var result = await query.ToListAsync();
            return result.Any() ? result : null;
        }

        public async Task<IEnumerable<T>> DynamicSearchProductsAsync(string sqlQuery)
        {
            IQueryable<T> query = _dbSet.AsQueryable();

            if (!string.IsNullOrWhiteSpace(sqlQuery))
            {
                try
                {
                    query = query.Where(sqlQuery); // Dynamic LINQ filter
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error executing dynamic search query: {Query}", sqlQuery);
                    return new List<T>(); // Return empty list on error
                }
            }

            var result = await query.ToListAsync();
            return result.Any() ? result : new List<T>();
        }
    }

}
