using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SolarCoffee.Data;
using SolarCoffee.Data.Enums;
using SolarCoffee.Data.Models;
using SolarCoffee.Services.Inventories;
using SolarCoffee.Services.Products;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SolarCoffee.Services.Orders
{
    public class OrderService : IOrderService
    {
        private readonly SolarDbContext _db;
        private readonly ILogger<OrderService> _logger;
        private readonly IProductService _productService;
        private readonly IInventoryService _inventoryService;

        public OrderService(SolarDbContext dbContext, ILogger<OrderService> logger, IProductService productService, IInventoryService inventoryService)
        {
            _db = dbContext;
            _logger = logger;
            _productService = productService;
            _inventoryService = inventoryService;
        }

        public async Task<List<SalesOrder>> GetOrders()
        {
            return await _db.SalesOrders
                .Include(o => o.Customer)
                .ThenInclude(c => c.PrimaryAddress)
                .Include(o=>o.SalesOrderItems)
                .ThenInclude(oi=>oi.Product)
                .Include(o => o.Payment)
                .Include(o => o.Discount)
                .Include(o => o.Delivery)
                .ToListAsync();
        }

        public async Task<bool> CreateOrder(SalesOrder order)
        {
            _logger.LogInformation("Generating new order");

            foreach (var item in order.SalesOrderItems)
            {
                await _inventoryService.UpdateUnitsAvailable(item.Product.Id, -item.Quantity);
            }

            try
            {
                await _db.SalesOrders.AddAsync(order);
                await _db.SaveChangesAsync();

                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> ChangeOrderStatus(int orderId, OrderStatus orderStatus)
        {
            var order = await _db.SalesOrders.FirstOrDefaultAsync(o => o.Id == orderId);

            //order.UpdatedOn = DateTime.Now;

            try
            {

                order.OrderStatus = orderStatus;

                await _db.SaveChangesAsync();

                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}