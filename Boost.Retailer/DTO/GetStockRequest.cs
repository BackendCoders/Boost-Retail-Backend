using Boost.Retail.Domain.Enums;

namespace BoostRetailLib.Data.DTO
{
    public class GetStockRequest
    {
        public string PartNumber { get; set; }
        public string LocationCode { get; set; } = AppConstants.ALL_LOCATIONS;
    }
}
