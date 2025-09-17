using AutoMapper;
using Boost.Retail.Data;
using Boost.Retail.Data.Models;
using Boost.Retail.Services.Interfaces;
using Microsoft.Extensions.Logging;


namespace Boost.Retail.Services
{
    public class LayawayService: ILayawayService
    {
        private readonly BoostDbContext _context;
        private readonly ILogger<Layaway> _logger;
        private readonly IMapper _mapper;
        private readonly IProductService _productService;
        private readonly IInventoryService _inventoryService;

        public LayawayService(ITenantDbContextFactory contextFactory, ILogger<Layaway> logger, IMapper mapper, IProductService productService, IInventoryService inventoryService)
        {
            _context = contextFactory.Create();
            _logger = logger;
            _mapper = mapper;
            _productService = productService;
            _inventoryService = inventoryService;
        }

        public async Task<IEnumerable<Layaway>> GetAllLayawayAsync(string customerAcc)
        {
            return new List<Layaway>();
        }


        public async Task<bool> AddLayawayAsync(Layaway layaway)
        {
            try
            {
                // Validate input
                if (layaway == null || string.IsNullOrEmpty(layaway.PartNumber) || layaway.Quantity <= 0)
                {
                    throw new ArgumentException("Invalid layaway data");
                }

                var parnumberExists = await _productService.PartNumberExistsAsync(layaway.PartNumber);
                if (!parnumberExists) 
                {
                    throw new ArgumentException("Invalid layaway data");
                }

                // Check if enough inventory exists
                var stock = await _inventoryService.GetStockAsync(layaway.PartNumber, layaway.LocationCode);
                bool isStockAvailable = stock >= layaway.Quantity;

                if (!isStockAvailable)
                {
                    throw new InvalidOperationException("Insufficient stock available");
                }

                // Subtract quantity from inventory
                int inventoryUpdated = await _inventoryService.AddStockAsync(
                    layaway.PartNumber,
                    layaway.LocationCode,
                    -layaway.Quantity,
                    new List<string> { layaway.StockNumber }
                    ); // Negative to reduce stock

               

                // Save layaway
                await _context.Layaways.AddAsync(layaway);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                // Log error (implement your logging mechanism)
                return false;
            }
        }

        // 2. Update product quantity in layaway
        public async Task<bool> UpdateLayawayQuantityAsync(int layawayId, int newQuantity)
        {
            try
            {
                if (newQuantity < 0)
                {
                    throw new ArgumentException("Quantity cannot be negative");
                }

                // Get existing layaway
                Layaway existingLayaway = await _context.Layaways.FindAsync(layawayId);
                if (existingLayaway == null)
                {
                    throw new InvalidOperationException("Layaway not found");
                }

                // Calculate quantity difference
                int quantityDifference = newQuantity - existingLayaway.Quantity;

                if (quantityDifference != 0)
                {
                    // Check if enough inventory exists for increase
                    if (quantityDifference > 0)
                    {
                        var stock = await _inventoryService.GetStockAsync(existingLayaway.PartNumber, existingLayaway.LocationCode);
                        bool isStockAvailable = stock >= quantityDifference;
                      
                        if (!isStockAvailable)
                        {
                            throw new InvalidOperationException("Insufficient stock available for update");
                        }
                    }

                    // Update inventory
                    int inventoryUpdated = await _inventoryService.AddStockAsync(
                        existingLayaway.PartNumber,
                         existingLayaway.LocationCode,
                         -quantityDifference,
                        new List<string> { existingLayaway.StockNumber } // Negative for increase, positive for decrease
                        );
                }

                // Update layaway quantity
                existingLayaway.Quantity = newQuantity;
                _context.Layaways.Update(existingLayaway);
               
                if (newQuantity == 0)
                {
                    _context.Layaways.Remove(existingLayaway);
                    await _context.SaveChangesAsync();
                }

                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                // Log error
                return false;
            }
        }

        // 3. Delete layaway
        public async Task<bool> DeleteLayawayAsync(int layawayId)
        {
            try
            {
                // Get existing layaway
                Layaway layaway = await _context.Layaways.FindAsync(layawayId);
                if (layaway == null)
                {
                    throw new InvalidOperationException("Layaway not found");
                }

                // Return quantity to inventory
                int inventoryUpdated = await _inventoryService.AddStockAsync(
                    layaway.PartNumber,
                    layaway.LocationCode,
                    layaway.Quantity, 
                    new List<string> { layaway.StockNumber } // Positive to add back to inventory
                    ); 

                // Delete layaway
                _context.Layaways.Remove(layaway);
                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                // Log error
                return false;
            }
        }
    }
}
