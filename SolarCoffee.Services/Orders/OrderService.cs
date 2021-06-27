using System;
using System.Collections;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SolarCoffee.Data;
using SolarCoffee.Data.Enums;
using SolarCoffee.Data.Models;
using SolarCoffee.Services.Inventories;
using SolarCoffee.Services.Products;
using System.Collections.Generic;
using System.Linq;
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

            var tasks = new List<Task>();

            foreach (var item in order.SalesOrderItems)
            {
                item.ProductId = item.Product.Id;


                tasks.Add(_inventoryService.UpdateUnitsAvailable(item.Product.Id, -item.Quantity));

                item.Product = null;
            }

            await Task.WhenAll(tasks.ToArray());
            //try
            //{
                order.DiscountId = order.Discount.Id;
                order.PaymentId = order.Payment.Id;
                order.CustomerId = order.Customer.Id;
                order.DeliveryId = order.Delivery.Id;

                order.Discount = null;
                order.Delivery = null;
                order.Payment = null;
                order.Customer = null;


                await db.SalesOrders.AddAsync(order);

                await db.SaveChangesAsync();

                return true;
            //}
            //catch
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

        public async Task<int> GetOrderItemNextId()
        {
            await using var db = new SolarDbContext();

            var lastOrderItemRow = await db.SalesOrderItems.OrderBy(p => p.Id).LastAsync();

            _logger.LogInformation(lastOrderItemRow.Id.ToString());

            return lastOrderItemRow.Id + 1;
        }
    }
}