namespace SolarCoffee.Web.ViewModels
{
    public record PaymentModel(int Id, string Name, string Description);

    public record DiscountModel(int Id, string Name, string Description, decimal DiscountPercent);

    public record DeliveryModel(int Id, string Name, string Description, decimal Price);

}