using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SolarCoffee.Data.Models;
using SolarCoffee.Services.Customer;

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
        public async Task<IActionResult> GetCustomers()
        {
            _logger.LogInformation("Getting all customers ...");
            var customers = await _customerService.GetAllCustomers();

            return Ok(customers);
        }

        [HttpGet("/api/customer/{id}")]
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
            _logger.LogInformation($"Adding new customer ...");

            var result = await _customerService.CreateCustomer(customer);

            return Ok(result);
        }

        [HttpPost("/api/customer/{id}")]
        [ProducesResponseType(typeof(Customer), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> RefreshCustomer(int id,[FromBody] Customer customer)
        {
            _logger.LogInformation($"Refreshing customer with id:{id}...");

            var result = await _customerService.RefreshCustomer(id,customer);

            return Ok(result);
        }
    }
}
