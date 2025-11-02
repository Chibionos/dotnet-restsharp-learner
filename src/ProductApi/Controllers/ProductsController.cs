using Microsoft.AspNetCore.Mvc;
using ProductApi.Models;

namespace ProductApi.Controllers;

/// <summary>
/// Controller for managing product resources
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    // In-memory data store (static so it persists across requests during app lifetime)
    private static readonly List<Product> _products = new()
    {
        new Product { Id = 1, Name = "Laptop", Price = 999.99m, Description = "High-performance laptop for professionals" },
        new Product { Id = 2, Name = "Mouse", Price = 29.99m, Description = "Wireless ergonomic mouse" },
        new Product { Id = 3, Name = "Keyboard", Price = 79.99m, Description = "Mechanical keyboard with RGB lighting" }
    };

    private static int _nextId = 4;

    private readonly ILogger<ProductsController> _logger;

    public ProductsController(ILogger<ProductsController> logger)
    {
        _logger = logger;
    }

    /// <summary>
    /// Get all products
    /// </summary>
    /// <returns>List of all products</returns>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<Product>), StatusCodes.Status200OK)]
    public ActionResult<IEnumerable<Product>> GetAllProducts()
    {
        _logger.LogInformation("Getting all products. Count: {Count}", _products.Count);
        return Ok(_products);
    }

    /// <summary>
    /// Get a specific product by ID
    /// </summary>
    /// <param name="id">Product ID</param>
    /// <returns>Product details</returns>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(Product), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public ActionResult<Product> GetProduct(int id)
    {
        _logger.LogInformation("Getting product with ID: {Id}", id);

        var product = _products.FirstOrDefault(p => p.Id == id);

        if (product == null)
        {
            _logger.LogWarning("Product with ID {Id} not found", id);
            return NotFound(new { message = $"Product with ID {id} not found" });
        }

        return Ok(product);
    }

    /// <summary>
    /// Create a new product
    /// </summary>
    /// <param name="product">Product details</param>
    /// <returns>Created product</returns>
    [HttpPost]
    [ProducesResponseType(typeof(Product), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public ActionResult<Product> CreateProduct([FromBody] Product product)
    {
        if (string.IsNullOrWhiteSpace(product.Name))
        {
            return BadRequest(new { message = "Product name is required" });
        }

        if (product.Price <= 0)
        {
            return BadRequest(new { message = "Product price must be greater than zero" });
        }

        product.Id = _nextId++;
        _products.Add(product);

        _logger.LogInformation("Created product with ID: {Id}, Name: {Name}", product.Id, product.Name);

        return CreatedAtAction(nameof(GetProduct), new { id = product.Id }, product);
    }

    /// <summary>
    /// Update an existing product
    /// </summary>
    /// <param name="id">Product ID</param>
    /// <param name="updatedProduct">Updated product details</param>
    /// <returns>Updated product</returns>
    [HttpPut("{id}")]
    [ProducesResponseType(typeof(Product), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public ActionResult<Product> UpdateProduct(int id, [FromBody] Product updatedProduct)
    {
        if (string.IsNullOrWhiteSpace(updatedProduct.Name))
        {
            return BadRequest(new { message = "Product name is required" });
        }

        if (updatedProduct.Price <= 0)
        {
            return BadRequest(new { message = "Product price must be greater than zero" });
        }

        var product = _products.FirstOrDefault(p => p.Id == id);

        if (product == null)
        {
            _logger.LogWarning("Product with ID {Id} not found for update", id);
            return NotFound(new { message = $"Product with ID {id} not found" });
        }

        product.Name = updatedProduct.Name;
        product.Price = updatedProduct.Price;
        product.Description = updatedProduct.Description;

        _logger.LogInformation("Updated product with ID: {Id}", id);

        return Ok(product);
    }

    /// <summary>
    /// Delete a product
    /// </summary>
    /// <param name="id">Product ID</param>
    /// <returns>No content</returns>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public IActionResult DeleteProduct(int id)
    {
        var product = _products.FirstOrDefault(p => p.Id == id);

        if (product == null)
        {
            _logger.LogWarning("Product with ID {Id} not found for deletion", id);
            return NotFound(new { message = $"Product with ID {id} not found" });
        }

        _products.Remove(product);
        _logger.LogInformation("Deleted product with ID: {Id}", id);

        return NoContent();
    }
}
