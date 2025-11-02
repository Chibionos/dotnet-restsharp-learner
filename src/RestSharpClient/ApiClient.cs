using RestSharp;
using System.Text.Json;

namespace RestSharpClient;

/// <summary>
/// Client for interacting with the Product API using RestSharp
/// </summary>
public class ApiClient
{
    private readonly RestClient _client;
    private readonly string _baseUrl;

    /// <summary>
    /// Initializes a new instance of the ApiClient
    /// </summary>
    /// <param name="baseUrl">Base URL of the API (e.g., https://localhost:5001)</param>
    public ApiClient(string baseUrl)
    {
        _baseUrl = baseUrl;
        var options = new RestClientOptions(baseUrl)
        {
            // Disable SSL certificate validation for local development
            RemoteCertificateValidationCallback = (sender, certificate, chain, errors) => true
        };
        _client = new RestClient(options);
    }

    /// <summary>
    /// Get all products from the API
    /// </summary>
    public async Task<List<Product>?> GetAllProductsAsync()
    {
        try
        {
            var request = new RestRequest("api/products", Method.Get);
            var response = await _client.ExecuteAsync<List<Product>>(request);

            if (response.IsSuccessful && response.Data != null)
            {
                Console.WriteLine($"✓ Successfully retrieved {response.Data.Count} products");
                return response.Data;
            }

            Console.WriteLine($"✗ Failed to get products. Status: {response.StatusCode}");
            return null;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"✗ Error getting products: {ex.Message}");
            return null;
        }
    }

    /// <summary>
    /// Get a single product by ID
    /// </summary>
    public async Task<Product?> GetProductByIdAsync(int id)
    {
        try
        {
            var request = new RestRequest($"api/products/{id}", Method.Get);
            var response = await _client.ExecuteAsync<Product>(request);

            if (response.IsSuccessful && response.Data != null)
            {
                Console.WriteLine($"✓ Successfully retrieved product: {response.Data.Name}");
                return response.Data;
            }

            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                Console.WriteLine($"✗ Product with ID {id} not found");
            }
            else
            {
                Console.WriteLine($"✗ Failed to get product. Status: {response.StatusCode}");
            }

            return null;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"✗ Error getting product: {ex.Message}");
            return null;
        }
    }

    /// <summary>
    /// Create a new product
    /// </summary>
    public async Task<Product?> CreateProductAsync(string name, decimal price, string description)
    {
        try
        {
            var newProduct = new { Name = name, Price = price, Description = description };
            var request = new RestRequest("api/products", Method.Post);
            request.AddJsonBody(newProduct);

            var response = await _client.ExecuteAsync<Product>(request);

            if (response.IsSuccessful && response.Data != null)
            {
                Console.WriteLine($"✓ Successfully created product with ID: {response.Data.Id}");
                return response.Data;
            }

            Console.WriteLine($"✗ Failed to create product. Status: {response.StatusCode}");
            if (!string.IsNullOrEmpty(response.Content))
            {
                Console.WriteLine($"  Response: {response.Content}");
            }
            return null;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"✗ Error creating product: {ex.Message}");
            return null;
        }
    }

    /// <summary>
    /// Update an existing product
    /// </summary>
    public async Task<Product?> UpdateProductAsync(int id, string name, decimal price, string description)
    {
        try
        {
            var updatedProduct = new { Name = name, Price = price, Description = description };
            var request = new RestRequest($"api/products/{id}", Method.Put);
            request.AddJsonBody(updatedProduct);

            var response = await _client.ExecuteAsync<Product>(request);

            if (response.IsSuccessful && response.Data != null)
            {
                Console.WriteLine($"✓ Successfully updated product ID: {id}");
                return response.Data;
            }

            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                Console.WriteLine($"✗ Product with ID {id} not found");
            }
            else
            {
                Console.WriteLine($"✗ Failed to update product. Status: {response.StatusCode}");
                if (!string.IsNullOrEmpty(response.Content))
                {
                    Console.WriteLine($"  Response: {response.Content}");
                }
            }

            return null;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"✗ Error updating product: {ex.Message}");
            return null;
        }
    }

    /// <summary>
    /// Delete a product by ID
    /// </summary>
    public async Task<bool> DeleteProductAsync(int id)
    {
        try
        {
            var request = new RestRequest($"api/products/{id}", Method.Delete);
            var response = await _client.ExecuteAsync(request);

            if (response.IsSuccessful)
            {
                Console.WriteLine($"✓ Successfully deleted product ID: {id}");
                return true;
            }

            if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                Console.WriteLine($"✗ Product with ID {id} not found");
            }
            else
            {
                Console.WriteLine($"✗ Failed to delete product. Status: {response.StatusCode}");
            }

            return false;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"✗ Error deleting product: {ex.Message}");
            return false;
        }
    }
}

/// <summary>
/// Product model matching the API
/// </summary>
public class Product
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public string Description { get; set; } = string.Empty;

    public override string ToString()
    {
        return $"[{Id}] {Name} - ${Price:F2}\n    {Description}";
    }
}
