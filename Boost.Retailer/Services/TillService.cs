using AutoMapper;
using Boost.Retail.Data;
using Boost.Retail.Data.DTO;
using Boost.Retail.Data.Models;
using Boost.Retail.Domain.Enums;
using Boost.Retail.Services.Interfaces;
using Microsoft.Extensions.Logging;

namespace Boost.Retail.Services
{
    public class TillService : ITillService
    {
        private readonly BoostDbContext _context;
        private readonly ILogger<Product> _logger;
        private IInventoryService _inventoryService;
        private IProductService _productService;

        public TillService(ITenantDbContextFactory contextFactory, ILogger<Product> logger, IInventoryService inventoryService, IProductService productService) 
        {
            _context = contextFactory.Create();
            _logger = logger;
            _inventoryService = inventoryService;
            _productService = productService;
        }

        public async Task<TillCustomer> GetTillCustomer(string customerAcc)
        {
            var customer = _context.Customers.Select(p => new TillCustomer
            {
                CustomerAccount = p.AccNo,
                FirstName = p.Firstname,
                SurName = p.Surname,
                Email = p.Email,
                Phone = p.Telephone,
                Mobile = p.Mobile,
                Category = p.Category,
                ClaimRef = "",
                CreditLimit = p.CreditLimit,
                DeliveryGoods = false,
                Details = "",
                LoyaltyCardNumber = p.LoyaltyNo,
                PurchaseOrderNumber = "",
            }).FirstOrDefault(p => p.CustomerAccount == customerAcc);

            if (customer != null) 
            {
                if (customer.Layaways == null)
                {
                    customer.Layaways = _context.Layaways.Where(l => l.CustomerAccount == customer.CustomerAccount).ToList();
                    if(customer.Layaways == null) 
                    {
                        customer.Layaways = new List<Layaway>();
                    }
                }
                return customer;
            }
            else
            {
                _logger.LogWarning($"Customer with account {customerAcc} not found.");
                return null;
            }
        }

        public async Task<TillProduct> GetTillProduct(string partNumber, string locCode)
        {
            var product = await _productService.GetByPartNumberAsync(partNumber);
            if (product != null)
            {
                var stockHere = await _inventoryService.GetStockAsync(partNumber, locCode);
                var stockTotal = await _inventoryService.GetStockAsync(partNumber, AppConstants.ALL_LOCATIONS);
                var tproduct = new TillProduct
                {
                    PartNumber = product.PartNumber,
                    Title = product.Search1,
                    Discount = product.Discount,
                    CostPrice = product.CostPrice,
                    IsPromo = product.IsUsingPromoPrice(),
                    StockHere = stockHere,
                    StockTotal = stockTotal
                };
                
                tproduct.Price = tproduct.IsPromo ? product.PromoPrice : product.StorePrice;
                return tproduct;
            }
            else
            {
                _logger.LogWarning($"Product with part number {partNumber} not found.");
                return null;
            }
        }

        public async Task<List<TillProductShortcutDTO>> GetTillProductShortcuts(string setID)
        {
            // Later we need get from Product Shortcuts table
            var products = _context.Products.Select(p => new TillProductShortcutDTO
            {
                SetID = setID,
                Title = p.Search1,
                Category = p.CatA,
                ImageURL = p.ImageMain,
                BackgroundColor = "",
                ForegroundColor = "",
                PartNumber = p.PartNumber,
                IsActive = true,
            }).Take(20).ToList();
            return products;
        }


    }
}
