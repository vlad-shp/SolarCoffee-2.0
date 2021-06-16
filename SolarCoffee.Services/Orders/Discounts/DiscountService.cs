using Microsoft.EntityFrameworkCore;
using SolarCoffee.Data;
using SolarCoffee.Data.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SolarCoffee.Services.Orders.Discounts
{
    public class DiscountService : IDiscountService
    {
        private SolarDbContext _db;

        public DiscountService(SolarDbContext db)
        {
            _db = db;
        }
        public async Task<List<Discount>> GetDiscountInstances()
        {
            return await _db.Discounts.ToListAsync();
        }

        public async Task<Discount> AddNewDiscountInstance(Discount newDiscountInstance)
        {
            await _db.AddAsync(newDiscountInstance);
            await _db.SaveChangesAsync();

            return newDiscountInstance;
        }

        public async Task<Discount> ChangeDiscountInstance(int discountInstanceId, Discount changedDiscountInstance)
        {
            var method = await _db.Discounts.FirstOrDefaultAsync(d => d.Id == discountInstanceId);

            if (method == null)
            {
                return null;
            }

            method.Description = changedDiscountInstance.Description;
            method.Name = changedDiscountInstance.Name;
            method.DiscountPercent = changedDiscountInstance.DiscountPercent;

            await _db.SaveChangesAsync();

            return method;
        }

        public async Task<Discount> GetDiscountInstanceById(int discountId)
        {
            return await _db.Discounts.FirstOrDefaultAsync(d => d.Id == discountId);
        }

        public async Task<bool> RemoveDiscountInstance(int discountInstanceId)
        {
            var instance = await _db.Discounts.FirstOrDefaultAsync(d => d.Id == discountInstanceId);

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