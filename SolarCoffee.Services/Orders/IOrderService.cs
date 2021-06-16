using SolarCoffee.Data.Enums;
using SolarCoffee.Data.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SolarCoffee.Services.Orders
{
    public interface IOrderService
    {
        Task<List<SalesOrder>> GetOrders();
        Task<bool> CreateOrder(SalesOrder order);
        Task<bool> ChangeOrderStatus(int orderId, OrderStatus orderStatus);
    }
}