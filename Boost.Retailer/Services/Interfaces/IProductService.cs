
using Boost.Retail.Data.DTO;
using Boost.Retail.Data.Models;

namespace Boost.Retail.Services.Interfaces
{
    public interface IProductService : IGenericService<Product, int>
    {
        Task<string> AddNewPartAsync(Product item);
        Task<int> UpdatePartAsync(Product item);
        Task<bool> PartNumberExistsAsync(string partNumber);
        Task<Product?> GetByPartNumberAsync(string partNumber);
        Task<Product?> GetByMFRPartNumberAsync(string mpn);
        Task<Product?> GetByBarcodeAsync(string barcode);

        Task<IEnumerable<ProductDto>> SearchProductsAsync(Dictionary<string, string> filters);
        Task<IEnumerable<ProductDto>> DynamicSearchProductsAsync(string sqlQuery);
        Task<IEnumerable<object>> DynamicSearchProductsAsync(string sqlQuery, params string[] propertiesToSelect);
    }
}
