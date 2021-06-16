using SolarCoffee.Data.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SolarCoffee.Services.Orders.Discounts
{
    public interface IDiscountService
    {
        Task<List<Discount>> GetDiscountInstances();
        Task<Discount> AddNewDiscountInstance(Discount newDiscountInstance);
        Task<Discount> ChangeDiscountInstance(int discountInstanceId, Discount changedDiscountInstance);
        Task<Discount> GetDiscountInstanceById(int discountId);
        Task<bool> RemoveDiscountInstance(int discountInstanceId);
    }
}