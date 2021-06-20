using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SolarCoffee.Data.Models;
using SolarCoffee.Services.Orders.Deliveries;
using SolarCoffee.Services.Orders.Discounts;
using SolarCoffee.Services.Orders.Payments;
using SolarCoffee.Web.ViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace SolarCoffee.Web.Controllers
{
    public class OrderSettingsController : Controller
    {
        private readonly ILogger<ProductController> _logger;
        private readonly IPaymentService _paymentService;
        private readonly IDeliveryService _deliveryService;
        private readonly IDiscountService _discountService;

        public OrderSettingsController(ILogger<ProductController> logger, IPaymentService paymentService, IDeliveryService deliveryService, IDiscountService discountService)
        {
            _logger = logger;
            _paymentService = paymentService;
            _deliveryService = deliveryService;
            _discountService = discountService;
        }

        [HttpGet("/api/order/settings")]
        [ProducesResponseType(typeof(List<OrderSettingsViewModel>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> GetOrderSettings()
        {

            var payments = await _paymentService.GetPaymentMethods();
            var discounts = await _discountService.GetDiscountInstances();
            var deliveries = await _deliveryService.GetDeliveryMethods();

            var paymentModels = payments.Select(payment => new PaymentModel(payment.Id, payment.Name, payment.Description)).ToList();
            var discountModels = discounts.Select(discount => new DiscountModel(discount.Id, discount.Name, discount.Description, discount.DiscountPercent)).ToList();
            var deliveryModels = deliveries.Select(delivery => new DeliveryModel(delivery.Id, delivery.Name, delivery.Description, delivery.Price)).ToList();

            return Ok(new OrderSettingsViewModel(paymentModels, discountModels, deliveryModels));
        }

        [HttpPost("/api/order/settings/addNewPayment")]
        [ProducesResponseType(typeof(PaymentModel), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> CreateNewPaymentMethod([FromBody] PaymentModel paymentModel)
        {

            await _paymentService.AddNewPaymentMethod(new Payment
            {
                Id = 0,
                Name = paymentModel.Name,
                Description = paymentModel.Description
            });

            return Ok(paymentModel);
        }

        [HttpDelete("/api/order/settings/removePayment/{id}")]
        [ProducesResponseType(typeof(bool), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> RemovePaymentMethod(int paymentMethodId)
        {
            var result = await _paymentService.RemovePaymentMethod(paymentMethodId);
            return result ? Ok() : NotFound();
        }

        [HttpPost("/api/order/settings/addNewDelivery")]
        [ProducesResponseType(typeof(PaymentModel), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> CreateNewDeliveryMethod([FromBody] DeliveryModel deliveryModel)
        {

            await _deliveryService.AddNewDeliveryMethod(new Delivery
            {
                Id = 0,
                Name = deliveryModel.Name,
                Description = deliveryModel.Description,
                Price = deliveryModel.Price
            });

            return Ok(deliveryModel);
        }

        [HttpDelete("/api/order/settings/removeDelivery/{id}")]
        [ProducesResponseType(typeof(bool), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> RemoveDeliveryMethod(int deliveryMethodId)
        {
            var result = await _deliveryService.RemoveDeliveryMethod(deliveryMethodId);
            return result ? Ok() : NotFound();
        }

        [HttpPost("/api/order/settings/addNewDiscount")]
        [ProducesResponseType(typeof(PaymentModel), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> CreateNewDiscountInstance([FromBody] DiscountModel discountModel)
        {

            await _discountService.AddNewDiscountInstance(new Discount
            {
                Id = 0,
                Name = discountModel.Name,
                Description = discountModel.Description,
                DiscountPercent = discountModel.DiscountPercent
            });

            return Ok(discountModel);
        }

        [HttpDelete("/api/order/settings/removeDiscount/{id}")]
        [ProducesResponseType(typeof(bool), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> RemoveDiscountInstance(int discountInstanceId)
        {
            var result = await _discountService.RemoveDiscountInstance(discountInstanceId);
            return result ? Ok() : NotFound();
        }
    }
}
