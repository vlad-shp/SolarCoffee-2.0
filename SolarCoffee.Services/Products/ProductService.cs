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
        private readonly SolarDbContext _db;
        private readonly ILogger<ProductService> _logger;

        public ProductService(SolarDbContext dbContext, ILogger<ProductService> logger)
        {
            _db = dbContext;
            _logger = logger;
        }

        public async Task<List<Product>> GetAllProducts()
        {
            var products = await _db.Products.ToListAsync();
            return products;
        }

        public async Task<Product> GetProductById(int productId)
        {
            var product = await _db.Products.FirstOrDefaultAsync(x => x.Id == productId);

            return product;
        }

        public async Task<Product> CreateProduct(Product product, int idealQuantityForSale, int startQuantity)
        {
            try
            {
                var productCopy = await _db.Customers.FirstOrDefaultAsync(x =>
                    x.Email.Contains(product.Name));

                if (productCopy != default)
                {
                    return null;
                }

                await _db.Products.AddAsync(product);

                var newInventory = new ProductInventory
                {
                    Id = 0,
                    Product = product,
                    QuantityOnHand = startQuantity,
                    IdealQuantity = idealQuantityForSale,
                    CreatedOn = DateTime.Now,
                    UpdatedOn = DateTime.Now
                };

                await _db.ProductInventories.AddAsync(newInventory);

                await _db.SaveChangesAsync();

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
                var product = await _db.Products.FirstOrDefaultAsync(x => x.Id == productId);
                product.IsArchived = true;
                await _db.SaveChangesAsync();

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
            var product = await _db.Products.FirstOrDefaultAsync(x => x.Id == productId);

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

                _db.Products.Update(product);

                await _db.SaveChangesAsync();

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