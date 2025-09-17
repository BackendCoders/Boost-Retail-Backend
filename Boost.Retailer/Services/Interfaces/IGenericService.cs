
namespace Boost.Retail.Services.Interfaces
{
    public interface IGenericService<T, TId> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<T> GetByIdAsync(TId id);
        Task<T> AddAsync(T item);
        Task UpdateAsync(TId id, T updatedItem);
        Task DeleteAsync(TId id);

        Task<IEnumerable<T>> SearchProductsAsync(Dictionary<string, string> filters);
        Task<IEnumerable<T>> DynamicSearchProductsAsync(string sqlQuery);
    }

}
