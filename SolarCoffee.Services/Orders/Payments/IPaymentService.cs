using SolarCoffee.Data.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SolarCoffee.Services.Orders.Payments
{
    public interface IPaymentService
    {
        Task<List<Payment>> GetPaymentMethods();
        Task<Payment> AddNewPaymentMethod(Payment newPaymentMethod);
        Task<Payment> ChangePaymentMethod(int paymentMethodId, Payment changedPaymentMethod);
        Task<Payment> GetPaymentMethodById(int paymentMethodId);
        Task<bool> RemovePaymentMethod(int paymentMethodId);
    }
}