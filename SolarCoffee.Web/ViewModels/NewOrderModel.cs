using System.Collections.Generic;
using SolarCoffee.Services.Models;

namespace SolarCoffee.Web.ViewModels
{
    public record NewOrderModel(
        int CustomerId,
        List<OrderItemModel> OrderItems,
        int OrderStatusId,
        int DeliveryId,
        int PaymentId,
        int DiscountId,
        string AdditionalInfo,
        decimal TotalPrice);


}