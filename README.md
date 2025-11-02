# .NET RestSharp API Learning Project

A comprehensive learning project demonstrating how to create REST APIs using **ASP.NET Core** and consume them using **RestSharp**, a popular HTTP client library for .NET.

## 📚 Table of Contents

- [Overview](#overview)
- [Features](#features)
- [Prerequisites](#prerequisites)
- [Project Structure](#project-structure)
- [Getting Started](#getting-started)
- [Running the Application](#running-the-application)
- [API Documentation](#api-documentation)
- [RestSharp Client Examples](#restsharp-client-examples)
- [Testing](#testing)
- [Learning Resources](#learning-resources)
- [Troubleshooting](#troubleshooting)

## 🎯 Overview

This project contains three main components:

1. **ProductApi** - ASP.NET Core Web API with CRUD operations
2. **RestSharpClient** - Interactive console application using RestSharp
3. **ProductApi.Tests** - XUnit integration tests with RestSharp

Perfect for developers who want to learn:
- Creating RESTful APIs with ASP.NET Core
- Consuming APIs using RestSharp
- HTTP methods (GET, POST, PUT, DELETE)
- Integration testing with RestSharp and XUnit
- API best practices and error handling

## ✨ Features

### Web API (ProductApi)
- ✅ Full CRUD operations for Product resource
- ✅ In-memory data store
- ✅ Swagger/OpenAPI documentation
- ✅ CORS enabled for local development
- ✅ Comprehensive logging
- ✅ Error handling with proper HTTP status codes
- ✅ XML documentation

### RestSharp Console Client
- ✅ Interactive menu-driven interface
- ✅ Examples of all HTTP methods
- ✅ Error handling and response parsing
- ✅ Automated demo mode
- ✅ Clear console output with emojis

### Testing
- ✅ 11 comprehensive integration tests
- ✅ Positive and negative test cases
- ✅ Full CRUD workflow testing
- ✅ Using WebApplicationFactory for in-memory testing

## 📋 Prerequisites

- [.NET 9.0 SDK](https://dotnet.microsoft.com/download) or later
- Code editor (Visual Studio, VS Code, or Rider)
- Terminal/Command Prompt
- Basic understanding of C# and REST APIs

## 🏗️ Project Structure

```
dotnet-restsharp-learner/
├── src/
│   ├── ProductApi/              # ASP.NET Core Web API
│   │   ├── Controllers/
│   │   │   └── ProductsController.cs    # API endpoints
│   │   ├── Models/
│   │   │   └── Product.cs               # Product model
│   │   └── Program.cs                   # API configuration
│   │
│   └── RestSharpClient/         # Console app using RestSharp
│       ├── ApiClient.cs                 # RestSharp client wrapper
│       └── Program.cs                   # Interactive menu
│
├── tests/
│   └── ProductApi.Tests/        # XUnit tests with RestSharp
│       └── ProductApiTests.cs           # Integration tests
│
├── dotnet-restsharp-learner.sln
├── PRD.md                       # Product Requirements Document
└── README.md                    # This file
```

## 🚀 Getting Started

### 1. Clone or Navigate to the Project

```bash
cd dotnet-restsharp-learner
```

### 2. Restore Dependencies

```bash
dotnet restore
```

### 3. Build the Solution

```bash
dotnet build
```

### 4. Run Tests

```bash
dotnet test
```

All 11 tests should pass! ✅

## 🎮 Running the Application

### Step 1: Start the API

Open a terminal and run:

```bash
cd src/ProductApi
dotnet run
```

The API will start on `https://localhost:5001`

**Important:** Keep this terminal open!

### Step 2: Access Swagger UI (Optional)

Open your browser and navigate to:
```
https://localhost:5001
```

You'll see the Swagger UI where you can explore and test the API endpoints interactively.

### Step 3: Run the RestSharp Client

Open a **new terminal** and run:

```bash
cd src/RestSharpClient
dotnet run
```

You'll see an interactive menu:

```
╔═══════════════════════════════════════════╗
║   RestSharp API Client Demo              ║
║   Learning REST API consumption           ║
╚═══════════════════════════════════════════╝

API Base URL: https://localhost:5001
Make sure the ProductApi is running before using this client!

═══════════════════════════════════════════
Choose an operation:
═══════════════════════════════════════════
1. Get All Products (GET)
2. Get Product by ID (GET)
3. Create New Product (POST)
4. Update Product (PUT)
5. Delete Product (DELETE)
6. Run All Operations Demo
0. Exit
═══════════════════════════════════════════
Enter your choice:
```

## 📖 API Documentation

### Base URL
```
https://localhost:5001
```

### Endpoints

#### GET /api/products
Get all products

**Response:** `200 OK`
```json
[
  {
    "id": 1,
    "name": "Laptop",
    "price": 999.99,
    "description": "High-performance laptop for professionals"
  },
  ...
]
```

#### GET /api/products/{id}
Get a specific product by ID

**Response:** `200 OK` or `404 Not Found`
```json
{
  "id": 1,
  "name": "Laptop",
  "price": 999.99,
  "description": "High-performance laptop for professionals"
}
```

#### POST /api/products
Create a new product

**Request Body:**
```json
{
  "name": "New Product",
  "price": 29.99,
  "description": "Product description"
}
```

**Response:** `201 Created`
```json
{
  "id": 4,
  "name": "New Product",
  "price": 29.99,
  "description": "Product description"
}
```

**Validation:**
- Name is required
- Price must be greater than 0

#### PUT /api/products/{id}
Update an existing product

**Request Body:**
```json
{
  "name": "Updated Product",
  "price": 39.99,
  "description": "Updated description"
}
```

**Response:** `200 OK` or `404 Not Found`

**Validation:**
- Name is required
- Price must be greater than 0

#### DELETE /api/products/{id}
Delete a product

**Response:** `204 No Content` or `404 Not Found`

### HTTP Status Codes

| Code | Meaning |
|------|---------|
| 200 | OK - Request succeeded |
| 201 | Created - Resource created successfully |
| 204 | No Content - Resource deleted successfully |
| 400 | Bad Request - Invalid input data |
| 404 | Not Found - Resource not found |

## 💡 RestSharp Client Examples

### Creating a RestClient

```csharp
var options = new RestClientOptions(baseUrl)
{
    RemoteCertificateValidationCallback = (sender, certificate, chain, errors) => true
};
var client = new RestClient(options);
```

### GET Request

```csharp
var request = new RestRequest("api/products", Method.Get);
var response = await client.ExecuteAsync<List<Product>>(request);

if (response.IsSuccessful)
{
    var products = response.Data;
}
```

### POST Request

```csharp
var newProduct = new { Name = "Test", Price = 99.99m, Description = "Test" };
var request = new RestRequest("api/products", Method.Post);
request.AddJsonBody(newProduct);

var response = await client.ExecuteAsync<Product>(request);
```

### PUT Request

```csharp
var updatedProduct = new { Name = "Updated", Price = 149.99m, Description = "Updated" };
var request = new RestRequest($"api/products/{id}", Method.Put);
request.AddJsonBody(updatedProduct);

var response = await client.ExecuteAsync<Product>(request);
```

### DELETE Request

```csharp
var request = new RestRequest($"api/products/{id}", Method.Delete);
var response = await client.ExecuteAsync(request);
```

## 🧪 Testing

### Run All Tests

```bash
dotnet test
```

### Run Tests with Detailed Output

```bash
dotnet test --verbosity normal
```

### Test Coverage

The test suite includes:

1. ✅ **GetAllProducts_ReturnsOkWithProducts** - Verify GET all products
2. ✅ **GetProductById_WithValidId_ReturnsProduct** - Verify GET by ID (success)
3. ✅ **GetProductById_WithInvalidId_ReturnsNotFound** - Verify GET by ID (404)
4. ✅ **CreateProduct_WithValidData_ReturnsCreatedProduct** - Verify POST (success)
5. ✅ **CreateProduct_WithInvalidData_ReturnsBadRequest** - Verify POST (validation)
6. ✅ **CreateProduct_WithInvalidPrice_ReturnsBadRequest** - Verify POST (price validation)
7. ✅ **UpdateProduct_WithValidData_ReturnsUpdatedProduct** - Verify PUT (success)
8. ✅ **UpdateProduct_WithInvalidId_ReturnsNotFound** - Verify PUT (404)
9. ✅ **DeleteProduct_WithValidId_ReturnsNoContent** - Verify DELETE (success)
10. ✅ **DeleteProduct_WithInvalidId_ReturnsNotFound** - Verify DELETE (404)
11. ✅ **CompleteWorkflow_CreateUpdateDelete_WorksCorrectly** - Full CRUD workflow

## 📚 Learning Resources

### RestSharp Key Concepts

1. **RestClient** - Main client for making HTTP requests
2. **RestRequest** - Represents an HTTP request
3. **Method Enum** - HTTP methods (Get, Post, Put, Delete, etc.)
4. **AddJsonBody()** - Serialize object to JSON and add to request body
5. **ExecuteAsync<T>()** - Execute request and deserialize response
6. **IsSuccessful** - Check if request succeeded (2xx status)
7. **StatusCode** - HTTP status code of the response
8. **Data** - Deserialized response data

### ASP.NET Core Concepts

1. **Controllers** - Handle HTTP requests
2. **Routing** - Map URLs to controller actions
3. **Model Binding** - Convert HTTP data to C# objects
4. **Status Code Results** - Return appropriate HTTP responses
5. **CORS** - Enable cross-origin requests
6. **Swagger/OpenAPI** - API documentation
7. **Dependency Injection** - ILogger and services

### Testing Concepts

1. **WebApplicationFactory** - In-memory test server
2. **Integration Testing** - Test entire request/response flow
3. **XUnit Facts** - Test methods
4. **Assertions** - Verify expected behavior
5. **Test Fixtures** - Share setup across tests

## 🔧 Troubleshooting

### Issue: "Failed to determine the https port for redirect"

**Solution:** This is a warning and can be ignored in development. The API works fine over HTTPS.

### Issue: Port already in use

**Solution:** Change the port in `src/ProductApi/Properties/launchSettings.json` and update the client's base URL.

### Issue: SSL Certificate Error

**Solution:** Trust the development certificate:
```bash
dotnet dev-certs https --trust
```

### Issue: Tests failing

**Solution:**
1. Make sure no other instance of the API is running
2. Run `dotnet clean` and `dotnet build`
3. Restart your IDE/terminal

### Issue: Cannot connect to API from client

**Solution:**
1. Verify the API is running (check terminal output)
2. Ensure the base URL in the client matches the API URL
3. Check firewall settings

## 🎓 Next Steps

Ready to expand your knowledge? Try these exercises:

1. **Add Authentication**
   - Implement JWT authentication
   - Add [Authorize] attributes

2. **Add a Database**
   - Replace in-memory store with SQL Server or PostgreSQL
   - Use Entity Framework Core

3. **Add Validation**
   - Use Data Annotations
   - Implement FluentValidation

4. **Add Pagination**
   - Implement paging for GET all products
   - Add query parameters for page size and number

5. **Add Filtering and Sorting**
   - Filter products by price range
   - Sort by name, price, etc.

6. **Containerize with Docker**
   - Create Dockerfile for the API
   - Use docker-compose for multi-container setup

## 📝 License

This is a learning project and is free to use for educational purposes.

## 🤝 Contributing

This is a learning project, but feel free to fork and experiment!

## 📧 Questions?

Review the code comments and PRD.md for detailed implementation notes.

---

**Happy Learning!** 🚀

Built with ❤️ using .NET 9.0, ASP.NET Core, and RestSharp
