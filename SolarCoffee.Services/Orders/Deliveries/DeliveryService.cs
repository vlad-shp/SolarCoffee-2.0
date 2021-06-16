using Microsoft.EntityFrameworkCore;
using SolarCoffee.Data;
using SolarCoffee.Data.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SolarCoffee.Services.Orders.Deliveries
{
    public class DeliveryService : IDeliveryService
    {
        private SolarDbContext _db;

        public DeliveryService(SolarDbContext db)
        {
            _db = db;
        }

        public async Task<List<Delivery>> GetDeliveryMethods()
        {
            return await _db.Deliveries.ToListAsync();
        }

        public async Task<Delivery> AddNewDeliveryMethod(Delivery newDeliveryMethod)
        {
            await _db.AddAsync(newDeliveryMethod);
            await _db.SaveChangesAsync();

            return newDeliveryMethod;
        }

        public async Task<Delivery> ChangeDeliveryMethod(int deliveryMethodId, Delivery changedDeliveryMethod)
        {
            var method = await _db.Deliveries.FirstOrDefaultAsync(d => d.Id == deliveryMethodId);

            if (method == null)
            {
                return null;
            }

            method.Description = changedDeliveryMethod.Description;
            method.Name = changedDeliveryMethod.Name;
            method.Price = changedDeliveryMethod.Price;

            await _db.SaveChangesAsync();

            return method;
        }

        public async Task<Delivery> GetDeliveryMethodById(int deliveryMethodId)
        {
            return await _db.Deliveries.FirstOrDefaultAsync(d => d.Id == deliveryMethodId);
        }

        public async Task<bool> RemoveDeliveryMethod(int deliveryMethodId)
        {
            var method = await _db.Deliveries.FirstOrDefaultAsync(d => d.Id == deliveryMethodId);

            if (method == null)
            {
                return false;
            }

            _db.Remove(method);

            await _db.SaveChangesAsync();

            return true;
        }
    }
}