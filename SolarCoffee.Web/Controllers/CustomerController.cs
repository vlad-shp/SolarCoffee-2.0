using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SolarCoffee.Data.Models;
using SolarCoffee.Services.Customers;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace SolarCoffee.Web.Controllers
{
    public class CustomerController : Controller
    {
        private readonly ILogger<CustomerController> _logger;
        private readonly ICustomerService _customerService;

        public CustomerController(ILogger<CustomerController> logger, ICustomerService customerService)
        {
            _customerService = customerService;
            _logger = logger;
        }

        [HttpGet("/api/customer")]
        [ProducesResponseType(typeof(List<Customer>), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetCustomers()
        {
            _logger.LogInformation("Getting all customers ...");

            var customers = await _customerService.GetAllCustomers();

            return Ok(customers);
        }

        [HttpGet("/api/customer/{id}")]
        [ProducesResponseType(typeof(Customer), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> GetCustomerById(int id)
        {
            _logger.LogInformation($"Getting customer with id:{id} ...");

            var customer = await _customerService.GetCustomerById(id);

            return Ok(customer);
        }

        [HttpPost("/api/customer/")]
        [ProducesResponseType(typeof(Customer), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> CreateCustomer([FromBody] Customer customer)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _logger.LogInformation($"Adding new customer ...");

            var result = await _customerService.CreateCustomer(customer);

            return result == null ? BadRequest() : Ok(result);
        }

        [HttpPatch("/api/customer/{id}")]
        [ProducesResponseType(typeof(Customer), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> RefreshCustomer(int id, [FromBody] Customer customer)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _logger.LogInformation($"Refreshing customer with id:{id}...");

            var result = await _customerService.RefreshCustomer(id, customer);

            return result == null ? BadRequest() : Ok(result);
        }

        [HttpDelete("/api/customer/{id}")]
        public async Task<IActionResult> RemoveCustomer(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _logger.LogInformation($"Removing customer with id:{id}...");

            var result = await _customerService.DeleteCustomer(id);

            return result ? NotFound() : Ok();
        }
    }
}
