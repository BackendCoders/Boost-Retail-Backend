using BoostRetail.Integrations.SConnect.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace BoostRetail.Integrations.SConnect.Data
{
    public class SConnectDbContext : DbContext
    {
        private readonly IConfiguration _config;

        public SConnectDbContext(DbContextOptions<SConnectDbContext> options, IConfiguration configuration)
        : base(options)
        {
            _config = configuration;

        }

        public DbSet<Models.InventoryItem> InventoryItems { get; set; }
        public DbSet<Models.Location> Locations { get; set; }
        public DbSet<Models.Transaction> Transactions { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure schema here if needed
            modelBuilder.Entity<Location>().ToTable("Settings");
           modelBuilder.Entity<InventoryItem>().ToTable("ProductItems");
            modelBuilder.Entity<Transaction>().ToTable("FTT05");
        }
    }
}
