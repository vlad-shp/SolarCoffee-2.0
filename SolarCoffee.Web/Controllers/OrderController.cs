using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SolarCoffee.Data.Enums;
using SolarCoffee.Data.Models;
using SolarCoffee.Services.Customers;
using SolarCoffee.Services.Orders;
using SolarCoffee.Services.Orders.Deliveries;
using SolarCoffee.Services.Orders.Discounts;
using SolarCoffee.Services.Orders.Payments;
using SolarCoffee.Services.Products;
using SolarCoffee.Web.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace SolarCoffee.Web.Controllers
{
    public class OrderController : Controller
    {

        private readonly ILogger<ProductController> _logger;
        private readonly IOrderService _orderService;
        private readonly ICustomerService _customerService;
        private readonly IPaymentService _paymentService;
        private readonly IDeliveryService _deliveryService;
        private readonly IDiscountService _discountService;
        private readonly IProductService _productService;

        public OrderController(ILogger<ProductController> logger, IOrderService orderService, ICustomerService customerService, IPaymentService paymentService, IDeliveryService deliveryService, IDiscountService discountService, IProductService productService)
        {
            _logger = logger;
            _orderService = orderService;
            _customerService = customerService;
            _paymentService = paymentService;
            _deliveryService = deliveryService;
            _discountService = discountService;
            _productService = productService;
        }

        [HttpGet("/api/order")]
        [ProducesResponseType(typeof(List<OrdersViewModel>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> GetOrders()
        {
            var orders = await _orderService.GetOrders();

            var orderViews = new List<OrdersViewModel>();

            foreach (var order in orders)
            {
                var salesOrderItems = order.SalesOrderItems.Select(orderItem => new OrderItemModel(orderItem.Id, orderItem.Quantity, orderItem.Product.Id)).ToList();

                orderViews.Add(new OrdersViewModel(
                    order.Id, 
                    order.Customer,
                    salesOrderItems,
                    order.OrderStatus,
                    new DeliveryModel(
                        order.Delivery.Id,
                        order.Delivery.Name,
                        order.Delivery.Description,
                        order.Delivery.Price),
                    new PaymentModel(
                        order.Payment.Id,
                        order.Payment.Name,
                        order.Payment.Description),
                    new DiscountModel(
                        order.Discount.Id,
                        order.Discount.Name,
                        order.Discount.Description,
                        order.Discount.DiscountPercent),
                    order.AdditionalInfo,
                    order.TotalPrice,
                    order.CreatedOn
                ));
            }

            return Ok(orderViews);
        }

        [HttpPatch("/api/order/change_status/{id}")]
        public async Task<ActionResult> ChangeOrderStatus(int id, OrderStatus orderStatusId)
        {
            _logger.LogInformation($"Changed newOrder[{id}] status to {orderStatusId}");

            var result = await _orderService.ChangeOrderStatus(id, orderStatusId);

            return !result ? NotFound() : Ok();
        }

        [HttpPost("/api/newOrder")]
        [ProducesResponseType(typeof(bool), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> CreateNewOrder([FromBody] NewOrderModel newOrder)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _logger.LogInformation("Generating new newOrder");


            var customer = await _customerService.GetCustomerById(newOrder.CustomerId);
            var orderStatus = (OrderStatus)newOrder.OrderStatusId;
            var payment = await _paymentService.GetPaymentMethodById(newOrder.PaymentId);
            var discount = await _discountService.GetDiscountInstanceById(newOrder.DiscountId);
            var delivery = await _deliveryService.GetDeliveryMethodById(newOrder.DeliveryId);

            var salesOrderItems = new List<SalesOrderItem>();

            foreach (var (id,quantity, productId) in newOrder.OrderItems)
            {
                var product = await _productService.GetProductById(productId);
                salesOrderItems.Add(new SalesOrderItem {Id = id, Quantity = quantity, Product = product});
            }

            var salesOrder = new SalesOrder { 
                Id = 0,
                Customer = customer,
                SalesOrderItems = salesOrderItems,
                OrderStatus = orderStatus,
                Delivery = delivery,
                Payment = payment,
                Discount = discount,
                AdditionalInfo = newOrder.AdditionalInfo,
                TotalPrice = newOrder.TotalPrice,
                CreatedOn = DateTime.Now,
                UpdatedOn = DateTime.Now};

            var result = await _orderService.CreateOrder(salesOrder);


            return !result ? BadRequest() : Ok();
        }
    }
}
