using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SolarCoffee.Data.Models;
using SolarCoffee.Services.Inventories;
using SolarCoffee.Web.ViewModels;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace SolarCoffee.Web.Controllers
{
    public class InventoryController : Controller
    {
        private readonly IInventoryService _inventoryService;
        private readonly ILogger<InventoryService> _logger;

        public InventoryController(
            ILogger<InventoryService> logger,
            IInventoryService inventoryService)
        {
            _inventoryService = inventoryService;
            _logger = logger;
        }

        [HttpGet("/api/inventory")]
        [ProducesResponseType(typeof(List<ProductInventory>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetCurrentInventory()
        {

            _logger.LogInformation("Getting all inventory ...");

            var inventory = await _inventoryService.GetInventory();

            return Ok(inventory);
        }

        [HttpPatch("/api/inventory")]
        [ProducesResponseType(typeof(List<ProductInventory>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> UpdateInventory([FromBody] ShipmentModel shipment)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _logger.LogInformation($"Updating inventory for {shipment.ProductId} :" +
                                   $"adjustment {shipment.Adjustment}");

            var inventory = await _inventoryService.UpdateUnitsAvailable(shipment.ProductId, shipment.Adjustment);

            return inventory == null ? NotFound() : Ok(inventory);
        }


    }
}
