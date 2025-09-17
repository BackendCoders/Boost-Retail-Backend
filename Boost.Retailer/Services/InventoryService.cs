
using Boost.Retail.Data;
using Boost.Retail.Data.Models;
using Boost.Retail.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;

namespace Boost.Retail.Services
{
    public class InventoryService : GenericService<Inventory, int>, IInventoryService
    {
        private readonly BoostDbContext _context;
        private readonly ILogger<Inventory> _logger;
        private readonly IProductService _productService;
        public InventoryService(ITenantDbContextFactory contextFactory, ILogger<Inventory> logger, IProductService productService) : base(() => contextFactory.Create(), logger)
        {
            _context = contextFactory.Create();
            _logger = logger;
            _productService = productService;
        }

        public async Task<int> GetStockAsync(string partNumber, string locationCode)
        {
            var inventory = await _context.Inventories.FirstOrDefaultAsync(p => p.PartNumber == partNumber);
            return inventory?.GetStockLevel(locationCode) ?? 0;
        }
        public async Task<int> AddStockAsync(string partNumber, string locationCode, int stock , List<string> stockNumbers)
        {
            var inventory = await _context.Inventories.FirstOrDefaultAsync(p => p.PartNumber == partNumber);
            if (inventory == null)
            {
                if (await _productService.PartNumberExistsAsync(partNumber))
                {
                    inventory = new Inventory
                    {
                        PartNumber = partNumber
                    };
                    _context.Inventories.Add(inventory);
                }
            }
           
            inventory?.SetStockLevel(locationCode, stock);
            await _context.SaveChangesAsync();
            return inventory?.GetStockLevel(locationCode) ?? 0;
        }
    }
}
