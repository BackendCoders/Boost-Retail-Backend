
using Boost.Retail.Data.Models;

namespace Boost.Retail.Services.Interfaces
{
    public interface IInventoryService : IGenericService<Inventory, int>
    {
        Task<int> GetStockAsync(string partNumber, string locationCode);
        Task<int> AddStockAsync(string partNumber, string locationCode, int stock, List<string> stocknumbers);
    }
}
