using Boost.Admin.Data;
using Boost.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;


namespace Boost.Retail.Data
{
    public interface ITenantDbContextFactory
    {
        BoostDbContext Create();
        Task<Guid> CreateTenant(Tenant tenant);
    }

    public class TenantDbContextFactory : ITenantDbContextFactory
    {
        private readonly SimDbContext _masterDb;

        public TenantDbContextFactory(SimDbContext masterDb)
        {
            _masterDb = masterDb;
        }

        public BoostDbContext Create()
        {
            var tenantId = TokenHelper.GetTenantIdFromToken(new DefaultHttpContext());
            var tenant = _masterDb.Tenants.FirstOrDefault(t => t.Id == tenantId);
            if (tenant == null) throw new Exception("Invalid tenant");

            var options = new DbContextOptionsBuilder<BoostDbContext>()
                .UseSqlServer(tenant.DbConnectionString)
                .Options;

            return new BoostDbContext(options);
        }

        public async Task<Guid> CreateTenant(Tenant tenant)
        {
            _masterDb.Tenants.Add(tenant);
            try
            {
                await _masterDb.SaveChangesAsync();
                return tenant.Id;
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as needed
                throw new Exception("Error creating tenant", ex);
            }
        }
    }

    public static class TokenHelper
    {
        public static Guid GetTenantIdFromToken(HttpContext httpContext)
        {
            var user = httpContext.User;

            if (user == null || !user.Identity.IsAuthenticated)
                throw new UnauthorizedAccessException("User is not authenticated.");

            var tenantIdClaim = user.Claims.FirstOrDefault(c => c.Type == "TenantId");

            if (tenantIdClaim == null)
                throw new UnauthorizedAccessException("Tenant ID claim not found.");

            return Guid.Parse(tenantIdClaim.Value);
        }
    }


}
