using System;
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
        private readonly ILogger<OrderService> _logger;
        private readonly IProductService _productService;
        private readonly IInventoryService _inventoryService;

        public OrderService(ILogger<OrderService> logger, IProductService productService, IInventoryService inventoryService)
        {
            _logger = logger;
            _productService = productService;
            _inventoryService = inventoryService;
        }

        public async Task<List<SalesOrder>> GetOrders()
        {
            await using var db = new SolarDbContext();
            var result = await db.SalesOrders
                .Include(o => o.Customer)
                .ThenInclude(c => c.PrimaryAddress)
                .Include(o => o.SalesOrderItems)
                .ThenInclude(oi => oi.Product)
                .Include(o => o.Payment)
                .Include(o => o.Discount)
                .Include(o => o.Delivery)
                .ToListAsync();

            return result;
        }

        public async Task<bool> CreateOrder(SalesOrder order)
        {
            await using var db = new SolarDbContext();
            _logger.LogInformation("Generating new order");

            foreach (var item in order.SalesOrderItems)
            {
                await _inventoryService.UpdateUnitsAvailable(item.Product.Id, -item.Quantity);
            }

            try
            {
                db.Update(order);

                await db.SaveChangesAsync();

                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> ChangeOrderStatus(int orderId, OrderStatus orderStatus)
        {
            await using var db = new SolarDbContext();
            var order = await db.SalesOrders.FirstOrDefaultAsync(o => o.Id == orderId);

            //order.UpdatedOn = DateTime.Now;

            try
            {

                order.OrderStatus = orderStatus;

                await db.SaveChangesAsync();

                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}