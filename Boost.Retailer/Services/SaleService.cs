
using AutoMapper;
using Boost.Retail.Data;
using Boost.Retail.Data.DTO;
using Boost.Retail.Data.Models;
using Boost.Retail.Domain.Enums;
using Boost.Retail.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Boost.Retail.Services
{
    public class SaleService : ISaleService
    {
        private readonly BoostDbContext _context;
        private readonly ILogger<Product> _logger;
        public SaleService(ITenantDbContextFactory contextFactory, ILogger<Product> logger)
        {
            _context = contextFactory.Create();
            _logger = logger;
        }

        public async Task<(bool Success, string Message, object Result)> CreateSaleAsync(SaleRequest request)
        {
            if (request == null || !request.Items.Any())
            {
                return (false, "Invalid sale request", null);
            }

            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                // Check stock availability
                foreach (var item in request.Items)
                {
                    var product = await _context.Products
                        .FirstOrDefaultAsync(p => p.PartNumber == item.PartNumber);

                    if (product == null)
                    {
                        return (false, $"Product {item.PartNumber} not found", null);
                    }

                    var stockQuantity = request.Location != null
                    ? GetStockQuantity(product.PartNumber, request.Location)
                    : 0;

                    if (stockQuantity < item.Quantity)
                    {
                        return (false, $"Insufficient stock for product {item.PartNumber}", null);
                    }
                }

                // Create sale transaction
                var saleTransaction = new SaleTransaction
                {
                    CustomerAcc = request.CustomerAccount,
                    PaymentDueDate = request.PaymentDueDate,
                    TillId = request.TillId,
                    Status = request.PaymentTypes.Any(o => o.PaymentType == PaymentType.Pending)
                        ? TransactionStatus.PendingPayment
                        : TransactionStatus.Completed,
                    StaffCode = request.SalesCode,
                    DiscountCode = request.DiscountCode,
                    Notes = request.Notes,
                    Location = request.Location,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    OrderNo = request.OrderNo,
                    InvoiceNumber = request.InvoiceNumber,
                    PaymentTypes = request.PaymentTypes
                };

                // Create sale items and update stock
                saleTransaction.SaleItems = request.Items.Select(item =>
                {
                    var saleItem = new SaleItem
                    {
                        PartNumber = item.PartNumber,
                        Quantity = item.Quantity,
                        UnitPrice = item.UnitPrice,
                        Discount = item.Discount,
                        VAT = item.VAT,
                        CostPrice = item.CostPrice,
                        IsPromo = item.IsPromo,
                        StockNumber = item.StockNumber,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = DateTime.UtcNow
                    };

                    // Update stock
                    var product = _context.Products
                        .First(p => p.PartNumber == item.PartNumber);

                    RemoveStock(product.PartNumber, item.Quantity, request.Location);
                   
                    return saleItem;
                }).ToList();

                // Calculate totals
                saleTransaction.TotalAmount = saleTransaction.SaleItems.Sum(i => i.UnitPrice);
                saleTransaction.TotalDiscount = saleTransaction.SaleItems.Sum(i => i.Discount);
                saleTransaction.TotalVAT = saleTransaction.SaleItems.Sum (i => i.VAT);
                saleTransaction.Net = saleTransaction.SaleItems.Sum(i => i.TotalPrice);
                saleTransaction.Net = saleTransaction.SaleItems.Sum(i => i.TotalPrice - i.CostPrice);
                saleTransaction.Location = request.Location;

                _context.SaleTransactions.Add(saleTransaction);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return (true, "Sale created successfully", new
                {
                    saleTransaction.TransactionReference,
                    saleTransaction.TotalAmount,
                    saleTransaction.Status
                });
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return (false, $"An error occurred: {ex.Message}", null);
            }
        }

        public async Task<(bool Success, string Message, object Result)> ProcessReturnAsync(ReturnRequest request)
        {
            if (request == null || request.ReturnQuantity <= 0)
            {
                return (false, "Invalid return request", null);
            }

            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                // Validate sale transaction and sale item
                var saleTransaction = await _context.SaleTransactions
                    .Include(t => t.SaleItems)
                    .FirstOrDefaultAsync(t => t.Id == request.SaleTransactionId);

                if (saleTransaction == null)
                {
                    return (false, "Sale transaction not found", null);
                }

                var saleItem = saleTransaction.SaleItems
                    .FirstOrDefault(i => i.Id == request.SaleItemId);

                if (saleItem == null)
                {
                    return (false, "Sale item not found", null);
                }

                // Check if return quantity is valid
                var totalReturned = await _context.ReturnTransactions
                    .Where(r => r.SaleItemId == request.SaleItemId)
                    .SumAsync(r => r.ReturnQuantity);

                if (totalReturned + request.ReturnQuantity > saleItem.Quantity)
                {
                    return (false, "Return quantity exceeds purchased quantity", null);
                }

                // Create return transaction
                var returnTransaction = new ReturnTransaction
                {
                    SaleTransactionId = request.SaleTransactionId,
                    SaleItemId = request.SaleItemId,
                    ReturnQuantity = request.ReturnQuantity,
                    RefundAmount = saleItem.UnitPrice * request.ReturnQuantity,
                    Reason = request.Reason,
                    TillId = request.TillId
                };

                // Update stock
                var product = await _context.Products
                    .FirstOrDefaultAsync(p => p.PartNumber == saleItem.PartNumber);
                if (product != null)
                {
                    AddStock(product.PartNumber, request.ReturnQuantity, request.Location);
                }

                // Update sale transaction status
                var totalItems = saleTransaction.SaleItems.Sum(i => i.Quantity);
                var totalReturnedItems = await _context.ReturnTransactions
                    .Where(r => r.SaleTransactionId == request.SaleTransactionId)
                    .SumAsync(r => r.ReturnQuantity) + request.ReturnQuantity;

                saleTransaction.Status = totalReturnedItems >= totalItems
                    ? TransactionStatus.Returned
                    : TransactionStatus.PartiallyReturned;

                saleTransaction.UpdatedAt = DateTime.UtcNow;

                _context.ReturnTransactions.Add(returnTransaction);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return (true, "Return processed successfully", new
                {
                    ReturnId = returnTransaction.Id,
                    returnTransaction.RefundAmount,
                    TransactionStatus = saleTransaction.Status
                });
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return (false, $"An error occurred: {ex.Message}", null);
            }
        }

        public async Task<(bool Success, string Message, object Result)> CompletePaymentAsync(CompletePaymentRequest request)
        {
            if (request == null)
            {
                return (false, "Invalid payment completion request", null);
            }

            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                var saleTransaction = await _context.SaleTransactions
                    .FirstOrDefaultAsync(t => t.TransactionReference == request.SaleTransactionRef);

                if (saleTransaction == null)
                {
                    return (false, "Sale transaction not found", null);
                }

                if (saleTransaction.Status != TransactionStatus.PendingPayment)
                {
                    return (false, "Transaction is not in pending payment status", null);
                }

                saleTransaction.PaymentTypes = request.PaymentTypes;
                saleTransaction.Status = request.PaymentTypes.Any(o=> o.PaymentType == PaymentType.Pending) 
                        ? TransactionStatus.PendingPayment
                        : TransactionStatus.Completed;
                saleTransaction.PaymentDueDate = null;
                saleTransaction.TillId = request.TillId;
                saleTransaction.UpdatedAt = DateTime.UtcNow;

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return (true, "Payment completed successfully", new
                {
                    saleTransaction.TransactionReference,
                    saleTransaction.Status
                });
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return (false, $"An error occurred: {ex.Message}", null);
            }
        }

        public async Task<(bool Success, string Message, object Result)> GetSaleTransactionAsync(string saleTransactionRef)
        {
            var saleTransaction = await _context.SaleTransactions
                .Include(t => t.SaleItems)
                .Include(t => t.Returns)
                .FirstOrDefaultAsync(t => t.TransactionReference == saleTransactionRef);

            if (saleTransaction == null)
            {
                return (false, "Sale transaction not found", null);
            }

            var result = new
            {
                saleTransaction.Id,
                saleTransaction.TransactionReference,
                saleTransaction.CustomerAcc,
                saleTransaction.TransactionDate,
                saleTransaction.Status,
                saleTransaction.TotalAmount,
                saleTransaction.TotalDiscount,
                saleTransaction.TotalVAT,
                saleTransaction.Profit,
                saleTransaction.Net,
                saleTransaction.Average,
                saleTransaction.InvoiceNumber,
                saleTransaction.OrderNo,
                saleTransaction.PaymentDueDate,
                saleTransaction.TillId,
                saleTransaction.CreatedAt,
                saleTransaction.UpdatedAt,
                saleTransaction.StaffCode,
                saleTransaction.DiscountCode,
                SaleItems = saleTransaction.SaleItems.Select(i => new
                {
                    i.Id,
                    i.PartNumber,
                    i.Quantity,
                    i.UnitPrice,
                    i.Discount,
                    i.VAT,
                    TotalPrice = i.TotalPrice,
                    i.StockNumber
                }),
                Returns = saleTransaction.Returns.Select(r => new
                {
                    r.Id,
                    r.SaleItemId,
                    r.ReturnQuantity,
                    r.RefundAmount,
                    r.Reason,
                    r.ReturnDate,
                    r.TillId
                }),
                PaymentTypes = saleTransaction.PaymentTypes.Select(p => new
                {
                    p.Id,
                    p.Type,
                    p.Amount,
                    p.PaymentType
                })
            };

            return (true, "Sale transaction retrieved successfully", result);
        }

        public async Task<(bool Success, string Message, object Result)> GetSaleTransactionsAsync(
            string? tillId = null,
            TransactionStatus? status = null,
            DateTime? startDate = null,
            DateTime? endDate = null,
            int page = 1,
            int pageSize = 10)
        {
            var query = _context.SaleTransactions
                .Include(t => t.CustomerAcc)
                .Include(t => t.SaleItems)
                .AsQueryable();

            if (!string.IsNullOrEmpty(tillId))
            {
                query = query.Where(t => t.TillId == tillId);
            }

            if (status.HasValue)
            {
                query = query.Where(t => t.Status == status.Value);
            }

            if (startDate.HasValue)
            {
                query = query.Where(t => t.TransactionDate >= startDate.Value);
            }

            if (endDate.HasValue)
            {
                query = query.Where(t => t.TransactionDate <= endDate.Value);
            }

            var totalCount = await query.CountAsync();
            var transactions = await query
                .OrderByDescending(t => t.TransactionDate)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(t => new
                {
                    t.Id,
                    t.TransactionReference,
                    t.CustomerAcc,
                    t.TransactionDate,
                    t.Status,
                    t.TotalAmount,
                    t.TotalDiscount,
                    t.TotalVAT,
                    t.PaymentDueDate,
                    t.TillId,
                    t.CreatedAt,
                    t.UpdatedAt,
                    ItemCount = t.SaleItems.Count,
                    ReturnCount = t.Returns.Count
                })
                .ToListAsync();

            var result = new
            {
                TotalCount = totalCount,
                Page = page,
                PageSize = pageSize,
                Transactions = transactions
            };

            return (true, "Sale transactions retrieved successfully", result);
        }
        

        public async Task<(bool Success, string Message, object Result)> CheckStockAsync(List<StockCheckRequest> requests)
        {
            if (requests == null || !requests.Any())
            {
                return (false, "Invalid stock check request", null);
            }

            var results = new List<object>();
            foreach (var request in requests)
            {
                var product = await _context.Products
                    .FirstOrDefaultAsync(p => p.PartNumber == request.PartNumber);

                if (product == null)
                {
                    results.Add(new
                    {
                        ProductPartNumber = request.PartNumber,
                        Available = false,
                        Message = "Product not found"
                    });
                    continue;
                }

                var stockQuantity = request.Location != null
                    ? GetStockQuantity(request.PartNumber, request.Location)
                    : 0;

                results.Add(new
                {
                    ProductPartNumber = request.PartNumber,
                    Available = stockQuantity >= request.Quantity,
                    StockQuantity = stockQuantity,
                    RequestedQuantity = request.Quantity
                });
            }

            return (true, "Stock check completed successfully", results);
        }

        private int GetStockQuantity(string partNumber, string loc)
        {
            var product = _context.Inventories
                .FirstOrDefault(p => p.PartNumber == partNumber);
            return product?.GetStockLevel(loc) ?? 0;
        }

        private int AddStock(string partNumber, int quantity, string loc)
        {
            var product = _context.Inventories.FirstOrDefault(p => p.PartNumber == partNumber);
            if (product == null) return 0;

            var stockLevel = product.GetStockLevel(loc);
           
            product.SetStockLevel(loc, quantity);

            _context.SaveChanges();
            return product.GetStockLevel(loc);
        }

        private int RemoveStock(string partNumber, int quantity, string loc)
        {
            var product = _context.Inventories.FirstOrDefault(p => p.PartNumber == partNumber);
            if (product == null) return 0;
            var stockLevel = product.GetStockLevel(loc);
            if (stockLevel < quantity) return 0;

            product.SetStockLevel(loc, quantity * -1);
            _context.SaveChanges();

            return product.GetStockLevel(loc);
        }



        public async Task<(bool Success, string Message, object Result)> GetSaleItemsAsync(string saleTransactionRef)
        {
            var saleTransaction = await _context.SaleTransactions
                .Include(t => t.SaleItems)
                .FirstOrDefaultAsync(t => t.TransactionReference == saleTransactionRef);
            if (saleTransaction == null)
            {
                return (false, "Sale transaction not found", null);
            }
            var result = saleTransaction.SaleItems.Select(i => new
            {
                i.Id,
                i.PartNumber,
                i.Quantity,
                i.UnitPrice, 
                i.Discount,
                i.VAT, 
                TotalPrice = i.TotalPrice
            }).ToList();
            return (true, "Sale items retrieved successfully", result);
        }

        public async Task<(bool Success, string Message, object Result)> GetSalesByLocationAsync(string location)
        {
            var sales = await _context.SaleTransactions
                .Where(t => t.Location == location)
                .ToListAsync();
            if (sales == null || !sales.Any())
            {
                return (false, "No sales found for the specified location", null);
            }
            return (true, "Sales retrieved successfully", sales);
        }

        public async Task<(bool Success, string Message, object Result)> GetSalesByDateRangeAsync(DateTime startDate, DateTime endDate, string location)
        {
            var sales = await _context.SaleTransactions
                .Where(t => t.TransactionDate >= startDate && t.TransactionDate <= endDate && t.Location == location)
                .ToListAsync();
            if (sales == null || !sales.Any())
            {
                return (false, "No sales found for the specified date range", null);
            }
            return (true, "Sales retrieved successfully", sales);
        }

        public async Task<(bool Success, string Message, object Result)> GetSalesByCustomerAsync(string customerAccount, string location)
        {
            var sales = await _context.SaleTransactions
                .Where(t => t.CustomerAcc == customerAccount && t.Location == location)
                .ToListAsync();
            if (sales == null || !sales.Any())
            {
                return (false, "No sales found for the specified customer", null);
            }
            return (true, "Sales retrieved successfully", sales);
        }

        public async Task<(bool Success, string Message, object Result)> GetSalesByPartNumberAsync(string partNumber, string location)
        {
            var sales = await _context.SaleTransactions
                .Include(t => t.SaleItems)
                .Where(t => t.Location == location && t.SaleItems.Any(i => i.PartNumber == partNumber))
                .ToListAsync();
            if (sales == null || !sales.Any())
            {
                return (false, "No sales found for the specified part number", null);
            }
            return (true, "Sales retrieved successfully", sales);

        }

       
    }
}
