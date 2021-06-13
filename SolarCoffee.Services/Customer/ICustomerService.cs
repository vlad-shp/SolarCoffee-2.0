using System.Collections.Generic;
using System.Threading.Tasks;

namespace SolarCoffee.Services.Customer
{
    public interface ICustomerService
    {
        Task<List<Data.Models.Customer>> GetAllCustomers();
        Task<ServiceResponse<Data.Models.Customer>> CreateCustomer(Data.Models.Customer customer);
        Task<ServiceResponse<bool>> DeleteCustomer(int id);
        Task<ServiceResponse<bool>> RefreshCustomer(int id, Data.Models.Customer refreshCustomer);
        Task<ServiceResponse<Data.Models.Customer>> GetCustomerById(int id);
    }
}