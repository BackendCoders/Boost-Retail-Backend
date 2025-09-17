using Boost.Admin.Data;
using Boost.Admin.DTOs;

namespace Boost.Admin.Logic
{
    public interface ICatalogueLogic
    {
        Task<bool> Exists(string mpn);
        Task<LogicResult<BarcodeDto>> GetBarcode(string mpn);
        Task<LogicResult<BarcodesDto>> GetBarcodes(List<string> mpns);
        Task<List<BarcodeDto>> GetBarcodesBySupplier(DataSupplier supplier);
        Task<List<string>> GetBrands(DataSupplier supplier, string year);
        Task<LogicResult<DescriptionDto>> GetDescription(string mpn);
        Task<LogicResult<DescriptionsDto>> GetDescriptions(List<string> mpns);
        Task<LogicResult<ImagesDto>> GetImages(string mpn);
        Task<LogicResult<ImagesDto>> GetImagesBulk(List<string> mpns);
        Task<int> GetProductCountByKeywordYear(string keyword, string year);
        Task<int> GetProductCountBySupplierBrand(DataSupplier supplier, string brand, string year);
        Task<int> GetProductCountBySupplierBrandKeywordYear(DataSupplier supplier, string brand, string keyword, string year);
        Task<int> GetProductCountBySupplierYear(DataSupplier supplier, string year);
        Task<int> GetProductCountBySupplierYearKeyword(DataSupplier supplier, string keyword, string year);
        Task<int> GetProductCountByYear(string year);
        Task<LogicResult<SIMProductDto>> GetProductByMpn(string mpn);
        Task<LogicResult<SIMProductsDto>> GetProductsByMpnBulk(List<string> mpns);
        Task<List<SIMProductDto>> GetProductsBySupplier(DataSupplier supplier);
        Task<List<SIMProductDto>> GetProductsBySupplierAndType(DataSupplier supplier, ProductType type);
        Task<List<SIMProductDto>> GetProductsBySupplier(DataSupplier supplier, int year);
        Task<List<SIMProductDto>> GetProductsBySupplierBrandByYear(DataSupplier supplier, string brand, int skip, int take, string year);
        Task<List<SIMProductDto>> GetProductsBySupplierYearKeyword(DataSupplier supplier, int skip, int take, string keyword, string year);
        Task<List<SIMProductDto>> GetProductsBySupplierYearKeywordBrand(DataSupplier supplier, string brand, int skip, int take, string keyword, string year);
        Task<List<SIMProductDto>> GetProductsBySupplierYearPaged(DataSupplier supplier, int skip, int take, string year);
        Task<List<SIMProductDto>> GetProductsByYearKeyword(int skip, int take, string keyword, string year);
        Task<List<string>> GetProductTypes();
        Task<List<string>> GetSuppliers();
        Task<List<string>> GetYears();
    }
}
