using SolarCoffee.Data.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SolarCoffee.Services.Products
{
    public interface IProductService
    {
        Task<List<Product>> GetAllProducts();
        Task<Product> GetProductById(int productId);
        Task<Product> CreateProduct(Product product, int idealQuantityForSale, int startQuantity);
        Task<Product> ArchiveProduct(int productId);
        Task<Product> RefreshProduct(int productId, Product refreshedProduct);
    }
}