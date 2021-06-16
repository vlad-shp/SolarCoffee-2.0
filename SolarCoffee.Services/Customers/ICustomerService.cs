using SolarCoffee.Data.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SolarCoffee.Services.Customers
{
    public interface ICustomerService
    {
        Task<List<Customer>> GetAllCustomers();
        Task<Customer> CreateCustomer(Customer customer);
        Task<bool> DeleteCustomer(int id);
        Task<Customer> RefreshCustomer(int id, Customer refreshCustomer);
        Task<Customer> GetCustomerById(int id);
    }
}