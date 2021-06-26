using Microsoft.EntityFrameworkCore;
using SolarCoffee.Data;
using SolarCoffee.Data.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SolarCoffee.Services.Orders.Discounts
{
    public class DiscountService : IDiscountService
    {
        //private SolarDbContext _db;

        //public DiscountService(SolarDbContext db)
        //{
        //    _db = db;
        //}
        public async Task<List<Discount>> GetDiscountInstances()
        {
            await using var db = new SolarDbContext();
            return await db.Discounts.ToListAsync();
        }

        public async Task<Discount> AddNewDiscountInstance(Discount newDiscountInstance)
        {
            await using var db = new SolarDbContext();
            await db.AddAsync(newDiscountInstance);
            await db.SaveChangesAsync();

            return newDiscountInstance;
        }

        public async Task<Discount> ChangeDiscountInstance(int discountInstanceId, Discount changedDiscountInstance)
        {
            await using var db = new SolarDbContext();
            var method = await db.Discounts.FirstOrDefaultAsync(d => d.Id == discountInstanceId);

            if (method == null)
            {
                return null;
            }

            method.Description = changedDiscountInstance.Description;
            method.Name = changedDiscountInstance.Name;
            method.DiscountPercent = changedDiscountInstance.DiscountPercent;

            await db.SaveChangesAsync();

            return method;
        }

        public async Task<Discount> GetDiscountInstanceById(int discountId)
        {
            await using var db = new SolarDbContext();
            return await db.Discounts.FirstOrDefaultAsync(d => d.Id == discountId);
        }

        public async Task<bool> RemoveDiscountInstance(int discountInstanceId)
        {
            await using var db = new SolarDbContext();
            var instance = await db.Discounts.FirstOrDefaultAsync(d => d.Id == discountInstanceId);

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