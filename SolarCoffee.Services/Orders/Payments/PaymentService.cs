using Microsoft.EntityFrameworkCore;
using SolarCoffee.Data;
using SolarCoffee.Data.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SolarCoffee.Services.Orders.Payments
{
    public class PaymentService : IPaymentService
    {
        //private SolarDbContext _db;

        //public PaymentService(SolarDbContext db)
        //{
        //    _db = db;
        //}

        public async Task<List<Payment>> GetPaymentMethods()
        {
            await using var db = new SolarDbContext();

            return await db.Payments.ToListAsync();
        }

        public async Task<Payment> AddNewPaymentMethod(Payment newPaymentMethod)
        {
            await using var db = new SolarDbContext();

            await db.AddAsync(newPaymentMethod);
            await db.SaveChangesAsync();

            return newPaymentMethod;
        }

        public async Task<Payment> ChangePaymentMethod(int paymentMethodId, Payment changedPaymentMethod)
        {
            await using var db = new SolarDbContext();

            var method = await db.Payments.FirstOrDefaultAsync(d => d.Id == paymentMethodId);

            if (method == null)
            {
                return null;
            }

            method.Description = changedPaymentMethod.Description;
            method.Name = changedPaymentMethod.Name;

            await db.SaveChangesAsync();

            return method;
        }

        public async Task<Payment> GetPaymentMethodById(int paymentMethodId)
        {
            await using var db = new SolarDbContext();
            return await db.Payments.FirstOrDefaultAsync(d => d.Id == paymentMethodId);
        }

        public async Task<bool> RemovePaymentMethod(int paymentMethodId)
        {
            await using var db = new SolarDbContext();

            var instance = await db.Payments.FirstOrDefaultAsync(d => d.Id == paymentMethodId);

            if (instance == null)
            {
                return false;
            }

            db.Remove(instance);

            await db.SaveChangesAsync();

            return true;
        }
    }
}