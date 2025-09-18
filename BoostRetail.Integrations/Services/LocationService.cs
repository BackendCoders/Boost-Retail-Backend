using BoostRetail.Integrations.SConnect.Data;
using BoostRetail.Integrations.SConnect.Data.Models;
using BoostRetail.Integrations.SConnect.DTOs;
using BoostRetail.Integrations.SConnect.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace BoostRetail.Integrations.SConnect.Services
{
    public class LocationService
    {
        private SConnectDbContext _ctx;
        private readonly IConfiguration _config;

        public LocationService(SConnectDbContext ctx, IConfiguration config)
        {
            _ctx = ctx;
            _config = config;
        }

        public async Task<List<LocationResponseDto>> GetLocations()
        {
            var created = new DateTime(2018,02,06,12,25,0).ToIso8601String();
            var updated = new DateTime(2026, 08, 15, 16, 15, 20).ToIso8601String();

            var lst = new List<LocationResponseDto>();

            var locs = _config.GetSection("Locations").Get<int[]>();

            var data = _ctx.Locations.AsEnumerable().Where(o=> locs.Contains(o.BranchId) &&
                !string.IsNullOrEmpty(o.SpecializedLocatorId)).ToList();

            foreach (var item in data)
            {
                var symbol = item.SpecializedLocatorId.Split("-")?[0];
                lst.Add(new LocationResponseDto
                {
                    ShopName = item.BranchName,
                    Street = item.Address1,
                    Street2 = item.Address2,
                    City = item.Address3,
                    State = "",
                    Zipcode = item.Postcode,
                    Country = "GB",
                    Email = item.GeneralEmailAddress,
                    Phone = item.MainTelephone,
                    DealerSymbol = symbol,
                    Symbol = item.SpecializedLocatorId,
                    CreatedAt = created,
                    UpdatedAt = updated
                });
            }

            return lst;
        }

        public record LocationIdRecord(int BranchId, string SpecLocatorId);

        public async Task<List<LocationIdRecord>> GetLocationIds()
        {
            var locs = _config.GetSection("Locations").Get<int[]>();

            var data = _ctx.Locations.Where(o => 
                !string.IsNullOrEmpty(o.SpecializedLocatorId)).AsEnumerable()
               .Select(o => new { o.BranchId, o.SpecializedLocatorId }) // EF can translate
               .Where(o => locs.Contains(o.BranchId))
               .ToList();

            return data
                .Select(o => new LocationIdRecord(o.BranchId, o.SpecializedLocatorId))
                .ToList();
        }
    }
}
