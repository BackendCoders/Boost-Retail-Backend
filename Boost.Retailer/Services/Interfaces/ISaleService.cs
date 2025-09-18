using Boost.Retail.Data.DTO;
using Boost.Retail.Domain.Enums;

namespace Boost.Retail.Services.Interfaces
{
    public interface ISaleService
    {
        Task<(bool Success, string Message, object Result)> CreateSaleAsync(SaleRequest request);
        Task<(bool Success, string Message, object Result)> ProcessReturnAsync(ReturnRequest request);
        Task<(bool Success, string Message, object Result)> CompletePaymentAsync(CompletePaymentRequest request);
        Task<(bool Success, string Message, object Result)> GetSaleTransactionAsync(string saleTransactionRef);
        Task<(bool Success, string Message, object Result)> GetSaleTransactionsAsync(
            string? tillId = null,
            TransactionStatus? status = null,
            DateTime? startDate = null,
            DateTime? endDate = null,
            int page = 1,
            int pageSize = 10);
        Task<(bool Success, string Message, object Result)> GetSaleItemsAsync(string saleTransactionRef);
        Task<(bool Success, string Message, object Result)> GetSalesByLocationAsync(string location);
        Task<(bool Success, string Message, object Result)> GetSalesByDateRangeAsync(DateTime startDate, DateTime endDate, string location);
        Task<(bool Success, string Message, object Result)> GetSalesByCustomerAsync(string customerAccount, string location);
        Task<(bool Success, string Message, object Result)> GetSalesByPartNumberAsync(string partNumber, string location);

    }
}
