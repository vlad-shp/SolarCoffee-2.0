using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SolarCoffee.Data;
using SolarCoffee.Data.Models;
using SolarCoffee.Services.Inventories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SolarCoffee.Services.Customers
{
    public class CustomerService : ICustomerService
    {
        private readonly SolarDbContext _db;
        private readonly ILogger<InventoryService> _logger;

        public CustomerService(SolarDbContext dbContext, ILogger<InventoryService> logger)
        {
            _db = dbContext;
            _logger = logger;
        }

        public async Task<Customer> CreateCustomer(Customer customer)
        {
            try
            {
                var customerCopy = await _db.Customers.FirstOrDefaultAsync(x =>
                    x.Email.Contains(customer.Email) || x.PhoneNumber.Contains(customer.PhoneNumber));
                if (customerCopy != default)
                {
                    return null;
                }
                await _db.Customers.AddAsync(customer);
                await _db.SaveChangesAsync();

                _logger.LogInformation("New customer was created");
                return customer;
            }
            catch
            {
                return null;
            }
        }

        public async Task<bool> DeleteCustomer(int id)
        {
            var customer = await _db.Customers.FirstOrDefaultAsync(x => x.Id == id);

            if (customer == default)
            {
                return false;
            }

            try
            {
                _db.Customers.Remove(customer);
                await _db.SaveChangesAsync();
                _logger.LogInformation($"Customer with id:{id} was removed");
                return true;
            }
            catch
            {
                return false;
            }

        }

        public async Task<List<Customer>> GetAllCustomers()
        {
            var customers = await _db.Customers.
                Include(customer => customer.PrimaryAddress).
                ToListAsync();
            return customers;
        }

        public async Task<Customer> GetCustomerById(int id)
        {
            var customer = await _db.Customers.FirstOrDefaultAsync(x => x.Id == id);

            return customer;
        }

        public async Task<Customer> RefreshCustomer(int id, Customer refreshCustomer)
        {
            var customer = await _db.Customers.FirstOrDefaultAsync(x => x.Id == id);

            if (customer == default)
            {
                return null;
            }

            try
            {
                customer.FirstName = refreshCustomer.FirstName;
                customer.LastName = refreshCustomer.LastName;
                customer.Email = refreshCustomer.Email;
                customer.PhoneNumber = refreshCustomer.Email;
                customer.UpdatedOn = refreshCustomer.CreatedOn;
                if (customer.PrimaryAddress != null)
                {
                    customer.PrimaryAddress.AddressLine1 = refreshCustomer.PrimaryAddress.AddressLine1;
                    customer.PrimaryAddress.AddressLine2 = refreshCustomer.PrimaryAddress.AddressLine2;
                    customer.PrimaryAddress.City = refreshCustomer.PrimaryAddress.City;
                    customer.PrimaryAddress.Country = refreshCustomer.PrimaryAddress.Country;
                    customer.PrimaryAddress.PostalCode = refreshCustomer.PrimaryAddress.PostalCode;
                    customer.PrimaryAddress.State = refreshCustomer.PrimaryAddress.State;
                    customer.PrimaryAddress.UpdatedOn = refreshCustomer.PrimaryAddress.CreatedOn;
                }
                else
                {
                    customer.PrimaryAddress = refreshCustomer.PrimaryAddress;
                }

                customer.UpdatedOn = DateTime.Now;

                await _db.SaveChangesAsync();

                _logger.LogInformation($"Customer with id:{id} was refreshed");

                return customer;
            }
            catch
            {
                return null;
            }
        }
    }
}