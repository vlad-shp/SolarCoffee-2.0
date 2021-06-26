using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SolarCoffee.Data;
using SolarCoffee.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SolarCoffee.Services.Inventories
{
    public class InventoryService : IInventoryService
    {
        private readonly ILogger<InventoryService> _logger;

        public InventoryService(ILogger<InventoryService> logger)
        {
            _logger = logger;
        }


        public async Task<List<ProductInventory>> GetInventory()
        {
            await using var db = new SolarDbContext();
            return await db.ProductInventories
                .Include(pi => pi.Product)
                //.Where(pi => !pi.Product.IsArchived)
                .ToListAsync();
        }

        public async Task<ProductInventory> UpdateUnitsAvailable(int id, int adjustment)
        {
            await using var db = new SolarDbContext();
            try
            {
                var inventory = await db.ProductInventories
                    .Include(inv => inv.Product)
                    .FirstOrDefaultAsync(inv => inv.Product.Id == id);


                if (inventory == null)
                {
                    return null;
                }

                inventory.QuantityOnHand += adjustment;

                inventory.UpdatedOn = DateTime.Now;

                await db.SaveChangesAsync();

                _logger.LogInformation($"Inventory item with id:{id} was updated");

                return inventory;
            }
            catch
            {
                return null;
            }
        }

        public async Task<ProductInventory> GetInventoryItemById(int productId)
        {
            await using var db = new SolarDbContext();
            return await db.ProductInventories
                .Include(inv => inv.Product)
                .FirstAsync(inv => inv.Product.Id == productId);
        }
    }
}