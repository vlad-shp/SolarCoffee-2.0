using Microsoft.EntityFrameworkCore;
using SolarCoffee.Data;
using SolarCoffee.Data.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SolarCoffee.Services.Orders.Deliveries
{
    public class DeliveryService : IDeliveryService
    {
        //private SolarDbContext _db;

        //public DeliveryService(SolarDbContext db)
        //{
        //    _db = db;
        //}

        public async Task<List<Delivery>> GetDeliveryMethods()
        {
            await using var db = new SolarDbContext();
            return await db.Deliveries.ToListAsync();
        }

        public async Task<Delivery> AddNewDeliveryMethod(Delivery newDeliveryMethod)
        {
            await using var db = new SolarDbContext();
            await db.AddAsync(newDeliveryMethod);
            await db.SaveChangesAsync();

            return newDeliveryMethod;
        }

        public async Task<Delivery> ChangeDeliveryMethod(int deliveryMethodId, Delivery changedDeliveryMethod)
        {
            await using var db = new SolarDbContext();
            var method = await db.Deliveries.FirstOrDefaultAsync(d => d.Id == deliveryMethodId);

            if (method == null)
            {
                return null;
            }

            method.Description = changedDeliveryMethod.Description;
            method.Name = changedDeliveryMethod.Name;
            method.Price = changedDeliveryMethod.Price;

            await db.SaveChangesAsync();

            return method;
        }

        public async Task<Delivery> GetDeliveryMethodById(int deliveryMethodId)
        {
            await using var db = new SolarDbContext();
            return await db.Deliveries.FirstOrDefaultAsync(d => d.Id == deliveryMethodId);
        }

        public async Task<bool> RemoveDeliveryMethod(int deliveryMethodId)
        {
            await using var db = new SolarDbContext();

            var method = await db.Deliveries.FirstOrDefaultAsync(d => d.Id == deliveryMethodId);

            if (method == null)
            {
                return false;
            }

            db.Remove(method);

            await db.SaveChangesAsync();

            return true;
        }
    }
}