namespace SolarCoffee.Services.Models
{
    public record OrderItemModel(
        int Id,
        int Quantity,
        int ProductId);
}