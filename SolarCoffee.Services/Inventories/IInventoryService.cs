using SolarCoffee.Data.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SolarCoffee.Services.Inventories
{
    public interface IInventoryService
    {
        Task<List<ProductInventory>> GetInventory();
        Task<ProductInventory> UpdateUnitsAvailable(int id, int adjustment);
        Task<ProductInventory> GetInventoryItemById(int productId);
    }
}