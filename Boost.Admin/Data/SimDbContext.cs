using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Boost.Admin.Data.Models;
using Boost.Admin.Data.Models.Catalog;
using System.Xml;
using Boost.Shared;

namespace Boost.Admin.Data
{
    public partial class SimDbContext : IdentityDbContext<ApplicationUser>
    {
        IConfiguration _config;

        public virtual DbSet<Tenant> Tenants { get; set; }
        public virtual DbSet<CatalogueItem> CatalogueItems { get; set; }
        public virtual DbSet<SupplierImportHistory> SupplierImportHistories { get; set; }
        public virtual DbSet<Colour> Colours { get; set; }
        public virtual DbSet<Size> Sizes { get; set; }
        public virtual DbSet<Brand> Brands { get; set; }
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<VatRate> VatRates { get; set; }
        public virtual DbSet<Specification> Specifications { get; set; }
        public virtual DbSet<Geometry> Geometrys { get; set; }
        public virtual DbSet<ShortDescription> ShortDescriptions { get; set; }
        public virtual DbSet<LongDescription> LongDescriptions { get; set; }
        public virtual DbSet<MasterProduct> MasterProducts { get; set; }
        public virtual DbSet<UserSupplierProductCategoryBrand> UserSupplierProductCategoryBrands { get; set; }  
        public virtual DbSet<CategoryMap> CategoryMaps { get; set; }
        public virtual DbSet<CategoryLookup> CategoryLookups { get; set; }
        public virtual DbSet<SupplierFeed> SupplierFeeds { get; set; }


        public SimDbContext(DbContextOptions<SimDbContext> options, IConfiguration configuration) 
        {
            _config = configuration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(_config.GetConnectionString("DefaultConnection"));
            }
            optionsBuilder.EnableSensitiveDataLogging();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Self-referencing relationship for category hierarchy
            modelBuilder.Entity<Category>()
                .HasOne(c => c.Parent)
                .WithMany(c => c.SubCategories)
                .HasForeignKey(c => c.ParentId)
                .OnDelete(DeleteBehavior.Restrict);

            // One-to-Many: A product belongs to a category
            modelBuilder.Entity<MasterProduct>()
                .HasOne(p => p.Category1)
                .WithMany(c => c.Cat1Products) // Different collection for each relationship
                .HasForeignKey(p => p.Category1Id)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<MasterProduct>()
                .HasOne(p => p.Category2)
                .WithMany(c => c.Cat2Products)
                .HasForeignKey(p => p.Category2Id)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<MasterProduct>()
                .HasOne(p => p.Category3)
                .WithMany(c => c.Cat3Products)
                .HasForeignKey(p => p.Category3Id)
                .OnDelete(DeleteBehavior.Restrict);

            foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            {
                relationship.DeleteBehavior = DeleteBehavior.Restrict;
            }

            base.OnModelCreating(modelBuilder);
        }
        
    }
}
