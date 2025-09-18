using Boost.Retail.Data.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Boost.Retail.Data   
{
    public class BoostDbContext : IdentityDbContext<AppUser>
    {
        IConfiguration _config;
        public BoostDbContext(DbContextOptions<BoostDbContext> options) : base(options)
        {
           
        }
        public BoostDbContext(DbContextOptions<BoostDbContext> options, IConfiguration configuration) : base(options)
        {
            _config = configuration;
        }

        public virtual DbSet<UserProduct> UserProducts { get; set; }
        public virtual DbSet<UserProductGroupOverride> UserProductGroupOverrides { get; set; }
        public virtual DbSet<UserProductOverride> UserProductOverrides { get; set; }

        public DbSet<Product> Products { get; set; }
        public DbSet<ProductLevel> ProductLevels { get; set; }
        public DbSet<ProductCategory> Categories { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Inventory> Inventories { get; set; }
        public DbSet<InventoryEntryRecord> InventoryEntryRecords { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<Make> Makes { get; set; }

        public DbSet<PurchaseOrderHeader> PurchaseOrderHeaders { get; set; }
        public DbSet<PurchaseOrderItem> PurchaseOrderItems { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }

        public DbSet<SaleTransaction> SaleTransactions { get; set; }
        public DbSet<SaleItem> SaleItems { get; set; }
        public DbSet<ReturnTransaction> ReturnTransactions { get; set; }
        public DbSet<Layaway> Layaways { get; set; }
        public DbSet<SalePayment> SalePayments { get; set; }
        public DbSet<TillPaymentType> TillPaymentTypes { get; set; }

        // Override this method to configure your database relationships
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure relationships here (e.g., foreign keys, cascading deletes)
            // Example:
            modelBuilder.Entity<Product>()
                .HasIndex(u => u.PartNumber)
                .IsUnique();

            modelBuilder.Entity<ProductLevel>()
                .HasIndex(u => u.PartNumber)
                .IsUnique();

            modelBuilder.Entity<Customer>()
                .HasIndex(u => u.AccNo)
                .IsUnique();

            modelBuilder.Entity<ProductCategory>()
                .HasIndex(u => u.Code)
                .IsUnique();

            modelBuilder.Entity<Make>()
                .HasIndex(u => u.Code)
                .IsUnique();

            modelBuilder.Entity<Location>()
                .HasIndex(u => u.Code)
                .IsUnique();

            modelBuilder.Entity<Supplier>()
                .HasIndex(u => u.AccountNo)
                .IsUnique();

            modelBuilder.Entity<Inventory>()
                .HasIndex(u => u.PartNumber)
                .IsUnique();

            modelBuilder.Entity<PurchaseOrderHeader>()
                .HasIndex(u => u.OrderNumber)
                .IsUnique();



            modelBuilder.Entity<SaleTransaction>()
                .Property(t => t.TotalAmount)
                .HasPrecision(18, 2);

            modelBuilder.Entity<SaleTransaction>()
                .Property(t => t.TotalDiscount)
                .HasPrecision(18, 2);

            modelBuilder.Entity<SaleTransaction>()
                .Property(t => t.TotalVAT)
                .HasPrecision(18, 2);

            modelBuilder.Entity<SaleItem>()
                .Property(i => i.UnitPrice)
                .HasPrecision(18, 2);

            modelBuilder.Entity<SaleItem>()
                .Property(i => i.Discount)
                .HasPrecision(18, 2);

            modelBuilder.Entity<SaleItem>()
                .Property(i => i.VAT)
                .HasPrecision(18, 2);

            modelBuilder.Entity<ReturnTransaction>()
                .Property(r => r.RefundAmount)
                .HasPrecision(18, 2);

            // Configure indexes for performance
            modelBuilder.Entity<SaleTransaction>()
                .HasIndex(t => t.TransactionReference)
                .IsUnique();

            modelBuilder.Entity<SaleTransaction>()
                .HasIndex(t => t.TransactionDate);

            modelBuilder.Entity<SaleTransaction>()
                .HasIndex(t => t.TillId);



            base.OnModelCreating(modelBuilder);
        }

    }
}
