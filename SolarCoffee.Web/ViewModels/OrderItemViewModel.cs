using SolarCoffee.Data.Models;

namespace SolarCoffee.Web.ViewModels
{
    public record OrderItemViewModel(
        int Id,
        int Quantity,
        Product Product);
}