
using Boost.Retail.Data.Models;

namespace Boost.Retail.Services.Interfaces
{
    public interface ILayawayService
    {
        Task<IEnumerable<Layaway>> GetAllLayawayAsync(string customerAcc);
        Task<bool> AddLayawayAsync(Layaway layaway);
        Task<bool> UpdateLayawayQuantityAsync(int layawayId, int newQuantity);
        Task<bool> DeleteLayawayAsync(int layawayId);
    }
}
