using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SolarCoffee.Data;
using SolarCoffee.Data.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SolarCoffee.Services.Products
{
    public class ProductService : IProductService
    {
        private readonly ILogger<ProductService> _logger;

        public ProductService(ILogger<ProductService> logger)
        {
            _logger = logger;
        }

        public async Task<List<Product>> GetAllProducts()
        {
            await using var db = new SolarDbContext();
            var products = await db.Products.ToListAsync();
            return products;
        }

        public async Task<Product> GetProductById(int productId)
        {
            await using var db = new SolarDbContext();
            var product = await db.Products.FirstOrDefaultAsync(x => x.Id == productId);

            return product;
        }

        public async Task<Product> CreateProduct(Product product, int idealQuantityForSale, int startQuantity)
        {

            try
            {
                await using var db = new SolarDbContext();
                var productCopy = await db.Customers.FirstOrDefaultAsync(x =>
                    x.Email.Contains(product.Name));

                if (productCopy != default)
                {
                    return null;
                }

                await db.Products.AddAsync(product);

                var newInventory = new ProductInventory
                {
                    Id = 0,
                    Product = product,
                    QuantityOnHand = startQuantity,
                    IdealQuantity = idealQuantityForSale,
                    CreatedOn = DateTime.Now,
                    UpdatedOn = DateTime.Now
                };

                await db.ProductInventories.AddAsync(newInventory);

                await db.SaveChangesAsync();

                _logger.LogInformation("New product was created");

                return product;
            }
            catch (Exception ex)
            {
                _logger.LogTrace("Product action [Create] was failed", ex.StackTrace);

                return null;
            }
        }

        public async Task<Product> ArchiveProduct(int productId)
        {
            try
            {
                await using var db = new SolarDbContext();
                var product = await db.Products.FirstOrDefaultAsync(x => x.Id == productId);
                product.IsArchived = true;
                await db.SaveChangesAsync();

                _logger.LogInformation($"Product with id:{productId} was archived");

                return product;
            }
            catch
            {
                return null;
            }
        }

        public async Task<Product> RefreshProduct(int productId, Product refreshedProduct)
        {
            await using var db = new SolarDbContext();
            var product = await db.Products.FirstOrDefaultAsync(x => x.Id == productId);

            if (product == default)
            {
                return null;
            }

            try
            {
                product.Name = refreshedProduct.Name;
                product.Description = refreshedProduct.Description;
                product.IsTaxable = refreshedProduct.IsTaxable;
                product.IsArchived = refreshedProduct.IsArchived;
                product.Price = refreshedProduct.Price;
                product.UpdatedOn = refreshedProduct.CreatedOn;

                db.Products.Update(product);

                await db.SaveChangesAsync();

                _logger.LogInformation($"Product with id:{productId} was refreshed");

                return product;
            }
            catch
            {
                return null;
            }
        }
    }
}