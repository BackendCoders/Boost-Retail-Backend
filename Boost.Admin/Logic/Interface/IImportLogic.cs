using Boost.Admin.Data;
using Boost.Admin.Data.Models;
using Boost.Admin.Data.Models.Catalog;

namespace Boost.Admin.Logic
{
    public interface IImportLogic
    {
        Task<int> ImportGiant(GiantProductType type, int year);
        Task<int> ImportTrek(SupplierProductType type);
        Task<int> ImportCube(SupplierProductType type);
        Task<int> ImportWhyte();
        Task<int> ImportMadison();
        Task<int> ImportSportline();
        Task<int> ImportRaleigh(SupplierProductType type);
        Task<List<ShortDescription>> GetOrInsertShortDescriptions(List<string> tocheck);
        Task<List<(string Brand, int Id)>> GetBrandTypes();
        Task<List<(string Supplier, int Id)>> GetSupplierTypes();
        Task InsertOrUpdateBulk(List<CatalogueItem> items);
        Task CreateRecord(DataSupplier supplier, int productCount);
        Task<SupplierImportHistory> GetLastImportRecord(DataSupplier supplier);

        #region Process Imported Data

        Task<List<VatRate>> GetWithAddVatRates();
        Task<List<Brand>> GetOrInsertBrands(List<string> tocheck);
        Task<List<Size>> GetOrInsertSizes(List<string> tocheck);
        Task<List<Colour>> GetOrInsertColors(List<string> tocheck);
        Task<Category> GetCategory(string categoryName);
        Task<List<Category>> ProcessStringCategory(string fullCategory);
        Task<List<Boost.Admin.Data.Models.Specification>> GetOrInsertSpecs(List<string> tocheck);
        Task<List<Boost.Admin.Data.Models.Geometry>> GetOrInsertGeos(List<string> tocheck);
        Task<List<LongDescription>> GetOrInsertLongDescriptions(List<string> tocheck);
        //Task<List<ShortDescription>> GetOrInsertShortDescriptions(List<string> tocheck);
        Task Save(Boost.Admin.Data.Models.Catalog.MasterProduct product);
        Task ImportBaseCategoriesFromCSV();
        Task ImportCategoryMapsFromCSV();
        #endregion

    }
}