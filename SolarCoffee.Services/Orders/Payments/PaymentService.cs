using Microsoft.EntityFrameworkCore;
using SolarCoffee.Data;
using SolarCoffee.Data.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SolarCoffee.Services.Orders.Payments
{
    public class PaymentService : IPaymentService
    {
        private SolarDbContext _db;

        public PaymentService(SolarDbContext db)
        {
            _db = db;
        }

        public async Task<List<Payment>> GetPaymentMethods()
        {
            return await _db.Payments.ToListAsync();
        }

        public async Task<Payment> AddNewPaymentMethod(Payment newPaymentMethod)
        {
            await _db.AddAsync(newPaymentMethod);
            await _db.SaveChangesAsync();

            return newPaymentMethod;
        }

        public async Task<Payment> ChangePaymentMethod(int paymentMethodId, Payment changedPaymentMethod)
        {
            var method = await _db.Payments.FirstOrDefaultAsync(d => d.Id == paymentMethodId);

            if (method == null)
            {
                return null;
            }

            method.Description = changedPaymentMethod.Description;
            method.Name = changedPaymentMethod.Name;

            await _db.SaveChangesAsync();

            return method;
        }

        public async Task<Payment> GetPaymentMethodById(int paymentMethodId)
        {
            return await _db.Payments.FirstOrDefaultAsync(d => d.Id == paymentMethodId);
        }

        public async Task<bool> RemovePaymentMethod(int paymentMethodId)
        {
            var instance = await _db.Payments.FirstOrDefaultAsync(d => d.Id == paymentMethodId);

            if (instance == null)
            {
                return false;
            }

            _db.Remove(instance);

            await _db.SaveChangesAsync();

            return true;
        }
    }
}