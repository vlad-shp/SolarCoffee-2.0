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
        private readonly SolarDbContext _db;
        private readonly ILogger<InventoryService> _logger;

        public InventoryService(SolarDbContext dbContext, ILogger<InventoryService> logger)
        {
            _db = dbContext;
            _logger = logger;
        }


        public async Task<List<ProductInventory>> GetInventory()
        {
            return await _db.ProductInventories
                .Include(pi => pi.Product)
                //.Where(pi => !pi.Product.IsArchived)
                .ToListAsync();
        }

        public async Task<ProductInventory> UpdateUnitsAvailable(int id, int adjustment)
        {
            try
            {
                var inventory = await _db.ProductInventories
                    .Include(inv => inv.Product)
                    .FirstOrDefaultAsync(inv => inv.Product.Id == id);


                if (inventory == null)
                {
                    return null;
                }

                inventory.QuantityOnHand += adjustment;

                inventory.UpdatedOn = DateTime.Now;

                await _db.SaveChangesAsync();

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
            return await _db.ProductInventories
                .Include(inv => inv.Product)
                .FirstAsync(inv => inv.Product.Id == productId);
        }
    }
}