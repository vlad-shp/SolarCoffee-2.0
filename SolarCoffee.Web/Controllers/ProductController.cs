using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SolarCoffee.Data.Models;
using SolarCoffee.Services.Products;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

namespace SolarCoffee.Web.Controllers
{
    public class ProductController : Controller
    {
        private readonly ILogger<ProductController> _logger;
        private readonly IProductService _productService;

        public ProductController(ILogger<ProductController> logger, IProductService productService)
        {
            _logger = logger;
            _productService = productService;
        }

        [HttpGet("/api/product")]
        [ProducesResponseType(typeof(List<Product>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> GetProduct()
        {
            _logger.LogInformation("Getting all products");

            var products = await _productService.GetAllProducts();

            return Ok(products);
        }

        [HttpGet("/api/product/{id}")]
        [ProducesResponseType(typeof(Product), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> GetProductById(int id)
        {
            _logger.LogInformation($"Getting product with id:{id}");

            var product = await _productService.GetProductById(id);

            return product == null ? NotFound() : Ok(product);
        }

        [HttpPatch("/api/product/{id}/archive")]
        [ProducesResponseType(typeof(Product), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> ArchiveProduct(int id)
        {
            _logger.LogInformation("Archiving product");

            var archiveResult = await _productService.ArchiveProduct(id);

            return archiveResult == null ? NotFound() : Ok(archiveResult);
        }

        [HttpPost("/api/product")]
        [ProducesResponseType(typeof(Product), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> AddProduct([FromBody] Product product, int idealQuantityForSale, int startQuantity)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _logger.LogInformation("Adding a new product");

            var newProductResponse = await _productService.CreateProduct(product, idealQuantityForSale, startQuantity);

            return newProductResponse == null ? NotFound() : Ok(newProductResponse);
        }

        [HttpPatch("/api/product/id")]
        [ProducesResponseType(typeof(Product), (int)HttpStatusCode.OK)]
        public async Task<ActionResult> RefreshProduct(int id, [FromBody] Product product)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _logger.LogInformation("Refresh product");

            var refreshProduct = await _productService.RefreshProduct(id, product);

            return refreshProduct == null ? NotFound() : Ok(refreshProduct);
        }


    }
}
