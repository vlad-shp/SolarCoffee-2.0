using SolarCoffee.Data.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SolarCoffee.Services.Orders.Deliveries
{
    public interface IDeliveryService
    {
        Task<List<Delivery>> GetDeliveryMethods();
        Task<Delivery> AddNewDeliveryMethod(Delivery newDeliveryMethod);
        Task<Delivery> ChangeDeliveryMethod(int deliveryMethodId, Delivery changedDeliveryMethod);
        Task<Delivery> GetDeliveryMethodById(int deliveryMethodId);
        Task<bool> RemoveDeliveryMethod(int deliveryMethodId);
    }
}