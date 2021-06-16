using SolarCoffee.Data.Models;
using System.Collections.Generic;

namespace SolarCoffee.Web.ViewModels
{
    public record OrderSettingsViewModel(
        List<PaymentModel> Payments,
        List<DiscountModel> Discounts,
        List<DeliveryModel> Deliveries);
}