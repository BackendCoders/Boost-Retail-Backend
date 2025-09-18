

using Boost.Retail.Data.DTO;

namespace Boost.Retail.Services.Interfaces
{
    public interface ITillService
    {
        Task<List<TillProductShortcutDTO>> GetTillProductShortcuts(string setID);
        Task<TillProduct> GetTillProduct(string partNumber, string locCode);
        Task<TillCustomer> GetTillCustomer(string customerAcc);
    }
}
