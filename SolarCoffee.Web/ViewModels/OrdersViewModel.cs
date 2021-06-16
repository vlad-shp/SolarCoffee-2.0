using System;
using System.Collections.Generic;
using SolarCoffee.Data.Enums;
using SolarCoffee.Data.Models;

namespace SolarCoffee.Web.ViewModels
{
    public record OrdersViewModel(
        int Id,
        Customer Customer,
        List<OrderItemModel> OrderItems,
        OrderStatus OrderStatus,
        DeliveryModel Delivery,
        PaymentModel Payment,
        DiscountModel Discount,
        string AdditionalInfo,
        decimal TotalPrice,
        DateTime CreatedOn
    );

}