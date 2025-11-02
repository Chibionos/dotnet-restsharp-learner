using Microsoft.AspNetCore.Mvc.Testing;
using RestSharp;
using System.Net;
using Xunit;

namespace ProductApi.Tests;

/// <summary>
/// Integration tests for Product API using RestSharp
/// </summary>
public class ProductApiTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly RestClient _client;
    private readonly WebApplicationFactory<Program> _factory;

    public ProductApiTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory;

        // Create HttpClient from the factory
        var httpClient = factory.CreateClient();

        // Create RestClient using the HttpClient from the factory
        var options = new RestClientOptions
        {
            BaseUrl = httpClient.BaseAddress,
            ConfigureMessageHandler = _ => new HttpClientHandler
            {
                // This is handled by the test server, but we keep it for clarity
                ServerCertificateCustomValidationCallback = (_, _, _, _) => true
            }
        };

        _client = new RestClient(httpClient, options);
    }

    [Fact]
    public async Task GetAllProducts_ReturnsOkWithProducts()
    {
        // Arrange
        var request = new RestRequest("api/products", Method.Get);

        // Act
        var response = await _client.ExecuteAsync<List<Product>>(request);

        // Assert
        Assert.True(response.IsSuccessful);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.NotNull(response.Data);
        Assert.NotEmpty(response.Data);
        Assert.True(response.Data.Count >= 3); // Should have at least 3 default products
    }

    [Fact]
    public async Task GetProductById_WithValidId_ReturnsProduct()
    {
        // Arrange
        var request = new RestRequest("api/products/1", Method.Get);

        // Act
        var response = await _client.ExecuteAsync<Product>(request);

        // Assert
        Assert.True(response.IsSuccessful);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.NotNull(response.Data);
        Assert.Equal(1, response.Data.Id);
        Assert.Equal("Laptop", response.Data.Name);
    }

    [Fact]
    public async Task GetProductById_WithInvalidId_ReturnsNotFound()
    {
        // Arrange
        var request = new RestRequest("api/products/9999", Method.Get);

        // Act
        var response = await _client.ExecuteAsync<Product>(request);

        // Assert
        Assert.False(response.IsSuccessful);
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task CreateProduct_WithValidData_ReturnsCreatedProduct()
    {
        // Arrange
        var newProduct = new
        {
            Name = "Test Product",
            Price = 99.99m,
            Description = "Test Description"
        };
        var request = new RestRequest("api/products", Method.Post);
        request.AddJsonBody(newProduct);

        // Act
        var response = await _client.ExecuteAsync<Product>(request);

        // Assert
        Assert.True(response.IsSuccessful);
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        Assert.NotNull(response.Data);
        Assert.Equal("Test Product", response.Data.Name);
        Assert.Equal(99.99m, response.Data.Price);
        Assert.Equal("Test Description", response.Data.Description);
        Assert.True(response.Data.Id > 0);
    }

    [Fact]
    public async Task CreateProduct_WithInvalidData_ReturnsBadRequest()
    {
        // Arrange - empty name
        var invalidProduct = new
        {
            Name = "",
            Price = 99.99m,
            Description = "Test Description"
        };
        var request = new RestRequest("api/products", Method.Post);
        request.AddJsonBody(invalidProduct);

        // Act
        var response = await _client.ExecuteAsync(request);

        // Assert
        Assert.False(response.IsSuccessful);
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task CreateProduct_WithInvalidPrice_ReturnsBadRequest()
    {
        // Arrange - zero price
        var invalidProduct = new
        {
            Name = "Test Product",
            Price = 0m,
            Description = "Test Description"
        };
        var request = new RestRequest("api/products", Method.Post);
        request.AddJsonBody(invalidProduct);

        // Act
        var response = await _client.ExecuteAsync(request);

        // Assert
        Assert.False(response.IsSuccessful);
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task UpdateProduct_WithValidData_ReturnsUpdatedProduct()
    {
        // Arrange - First create a product
        var createProduct = new
        {
            Name = "Original Product",
            Price = 50.00m,
            Description = "Original Description"
        };
        var createRequest = new RestRequest("api/products", Method.Post);
        createRequest.AddJsonBody(createProduct);
        var createResponse = await _client.ExecuteAsync<Product>(createRequest);
        var createdProductId = createResponse.Data!.Id;

        // Arrange - Update data
        var updatedProduct = new
        {
            Name = "Updated Product",
            Price = 75.00m,
            Description = "Updated Description"
        };
        var updateRequest = new RestRequest($"api/products/{createdProductId}", Method.Put);
        updateRequest.AddJsonBody(updatedProduct);

        // Act
        var response = await _client.ExecuteAsync<Product>(updateRequest);

        // Assert
        Assert.True(response.IsSuccessful);
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.NotNull(response.Data);
        Assert.Equal("Updated Product", response.Data.Name);
        Assert.Equal(75.00m, response.Data.Price);
        Assert.Equal("Updated Description", response.Data.Description);
    }

    [Fact]
    public async Task UpdateProduct_WithInvalidId_ReturnsNotFound()
    {
        // Arrange
        var updatedProduct = new
        {
            Name = "Updated Product",
            Price = 75.00m,
            Description = "Updated Description"
        };
        var request = new RestRequest("api/products/9999", Method.Put);
        request.AddJsonBody(updatedProduct);

        // Act
        var response = await _client.ExecuteAsync<Product>(request);

        // Assert
        Assert.False(response.IsSuccessful);
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task DeleteProduct_WithValidId_ReturnsNoContent()
    {
        // Arrange - First create a product
        var createProduct = new
        {
            Name = "Product to Delete",
            Price = 25.00m,
            Description = "Will be deleted"
        };
        var createRequest = new RestRequest("api/products", Method.Post);
        createRequest.AddJsonBody(createProduct);
        var createResponse = await _client.ExecuteAsync<Product>(createRequest);
        var createdProductId = createResponse.Data!.Id;

        // Arrange - Delete request
        var deleteRequest = new RestRequest($"api/products/{createdProductId}", Method.Delete);

        // Act
        var response = await _client.ExecuteAsync(deleteRequest);

        // Assert
        Assert.True(response.IsSuccessful);
        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);

        // Verify product is deleted
        var getRequest = new RestRequest($"api/products/{createdProductId}", Method.Get);
        var getResponse = await _client.ExecuteAsync<Product>(getRequest);
        Assert.Equal(HttpStatusCode.NotFound, getResponse.StatusCode);
    }

    [Fact]
    public async Task DeleteProduct_WithInvalidId_ReturnsNotFound()
    {
        // Arrange
        var request = new RestRequest("api/products/9999", Method.Delete);

        // Act
        var response = await _client.ExecuteAsync(request);

        // Assert
        Assert.False(response.IsSuccessful);
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task CompleteWorkflow_CreateUpdateDelete_WorksCorrectly()
    {
        // 1. Create a product
        var createProduct = new
        {
            Name = "Workflow Test Product",
            Price = 100.00m,
            Description = "Testing complete workflow"
        };
        var createRequest = new RestRequest("api/products", Method.Post);
        createRequest.AddJsonBody(createProduct);
        var createResponse = await _client.ExecuteAsync<Product>(createRequest);

        Assert.True(createResponse.IsSuccessful);
        var productId = createResponse.Data!.Id;

        // 2. Retrieve the product
        var getRequest = new RestRequest($"api/products/{productId}", Method.Get);
        var getResponse = await _client.ExecuteAsync<Product>(getRequest);

        Assert.True(getResponse.IsSuccessful);
        Assert.Equal("Workflow Test Product", getResponse.Data!.Name);

        // 3. Update the product
        var updateProduct = new
        {
            Name = "Updated Workflow Product",
            Price = 150.00m,
            Description = "Updated in workflow"
        };
        var updateRequest = new RestRequest($"api/products/{productId}", Method.Put);
        updateRequest.AddJsonBody(updateProduct);
        var updateResponse = await _client.ExecuteAsync<Product>(updateRequest);

        Assert.True(updateResponse.IsSuccessful);
        Assert.Equal("Updated Workflow Product", updateResponse.Data!.Name);

        // 4. Delete the product
        var deleteRequest = new RestRequest($"api/products/{productId}", Method.Delete);
        var deleteResponse = await _client.ExecuteAsync(deleteRequest);

        Assert.True(deleteResponse.IsSuccessful);
        Assert.Equal(HttpStatusCode.NoContent, deleteResponse.StatusCode);

        // 5. Verify deletion
        var verifyRequest = new RestRequest($"api/products/{productId}", Method.Get);
        var verifyResponse = await _client.ExecuteAsync<Product>(verifyRequest);

        Assert.Equal(HttpStatusCode.NotFound, verifyResponse.StatusCode);
    }
}

/// <summary>
/// Product model for testing
/// </summary>
public class Product
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public string Description { get; set; } = string.Empty;
}
