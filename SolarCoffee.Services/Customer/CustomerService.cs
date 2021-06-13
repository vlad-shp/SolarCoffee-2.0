using System;
using SolarCoffee.Data;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace SolarCoffee.Services.Customer
{
    public class CustomerService:ICustomerService
    {
        private readonly SolarDbContext _db;

        public CustomerService(SolarDbContext dbContext)
        {
            _db = dbContext;
        }

        public async Task<ServiceResponse<Data.Models.Customer>> CreateCustomer(Data.Models.Customer customer)
        {
            try
            {
                var customerCopy = await _db.Customers.FirstOrDefaultAsync(x =>
                    x.Email.Contains(customer.Email) || x.PhoneNumber.Contains(customer.PhoneNumber));
                if (customerCopy != default)
                {
                    return new ServiceResponse<Data.Models.Customer>
                    {
                        IsSuccess = false,
                        Message = "Customer already was exist",
                        Time = DateTime.Now,
                        Data = customer
                    };
                }
                await _db.Customers.AddAsync(customer);
                await _db.SaveChangesAsync();
                return new ServiceResponse<Data.Models.Customer>
                {
                    IsSuccess = true,
                    Message = "New customer created",
                    Time = DateTime.Now,
                    Data = customer
                };
            }
            catch (Exception e)
            {
                return new ServiceResponse<Data.Models.Customer>
                {
                    IsSuccess = false,
                    Message = e.StackTrace,
                    Time = DateTime.Now,
                    Data = customer
                };
            }
        }

        public async Task<ServiceResponse<bool>> DeleteCustomer(int id)
        {
            var customer = await _db.Customers.FirstOrDefaultAsync(x=>x.Id==id);

            if (customer == default)
            {
                return new ServiceResponse<bool>
                {
                    Time = DateTime.Now,
                    IsSuccess = false,
                    Message = "Customer to delete not found!",
                    Data = false
                };
            }

            try
            {
                _db.Customers.Remove(customer);
                await _db.SaveChangesAsync();
                return new ServiceResponse<bool>
                {
                    Time = DateTime.Now,
                    IsSuccess = true,
                    Message = "Customer was deleted!",
                    Data = true
                };
            }
            catch (Exception e)
            {
                return new ServiceResponse<bool>
                {
                    Time = DateTime.Now,
                    IsSuccess = false,
                    Message = e.StackTrace,
                    Data = false
                };
            }

        }

        public async Task<List<Data.Models.Customer>> GetAllCustomers()
        {
            return await _db.Customers.
                Include(customer => customer.PrimaryAddress).
                ToListAsync();
        }

        public async Task<ServiceResponse<Data.Models.Customer>> GetCustomerById(int id)
        {
            var customer = await _db.Customers.FirstOrDefaultAsync(x=>x.Id==id);

            if (customer == default)
            {
                return new ServiceResponse<Data.Models.Customer>
                {
                    Time = DateTime.Now,
                    IsSuccess = false,
                    Message = "Customer not found!",
                    Data = null
                };
            }

            return new ServiceResponse<Data.Models.Customer>
            {
                Time = DateTime.Now,
                IsSuccess = true,
                Message = "Customer found!",
                Data = customer
            };
        }

        public async Task<ServiceResponse<bool>> RefreshCustomer(int id, Data.Models.Customer refreshCustomer)
        {
            var customer = await _db.Customers.FirstOrDefaultAsync(x => x.Id == id);
            
            if (customer == default)
            {
                return new ServiceResponse<bool>
                {
                    Time = DateTime.Now,
                    IsSuccess = false,
                    Message = "Customer not found!",
                    Data = false
                };
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

                _db.Customers.Update(customer);

                await _db.SaveChangesAsync();

                return new ServiceResponse<bool>
                {
                    Time = DateTime.Now,
                    IsSuccess = true,
                    Message = "Customer data was refreshed!",
                    Data = true
                };
            }
            catch (Exception e)
            {
                return new ServiceResponse<bool>
                {
                    IsSuccess = false,
                    Message = e.StackTrace,
                    Time = DateTime.Now,
                    Data = false
                };
            }
        }
    }
}