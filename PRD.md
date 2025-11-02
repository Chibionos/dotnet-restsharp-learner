# Product Requirements Document: .NET RestSharp API Learning Project

## 📋 Overview
This project demonstrates how to create REST APIs using ASP.NET Core and test them using RestSharp, a popular HTTP client library for .NET.

## 🎯 Objectives
- Understand REST API fundamentals in .NET
- Learn how to create a simple Web API using ASP.NET Core
- Master RestSharp for API consumption and testing
- Practice CRUD operations (Create, Read, Update, Delete)
- Explore different HTTP methods (GET, POST, PUT, DELETE)

## 🏗️ Architecture

### Components
1. **ASP.NET Core Web API** (Server)
   - Simple REST API exposing endpoints for a "Product" resource
   - In-memory data storage
   - Standard CRUD operations
   - Runs on `https://localhost:5001`

2. **RestSharp Console Client** (Client)
   - Console application using RestSharp
   - Demonstrates how to consume the API
   - Examples of all HTTP methods
   - Error handling and response parsing

3. **XUnit Test Project** (Testing)
   - Integration tests using RestSharp
   - Test all API endpoints
   - Assert response status and data

## 📦 Technology Stack
- **.NET 8.0** (or latest available)
- **ASP.NET Core Web API** for server
- **RestSharp** for HTTP client operations
- **XUnit** for testing
- **C#** programming language

## 🔧 Project Structure
```
dotnet-restsharp-learner/
├── src/
│   ├── ProductApi/              # ASP.NET Core Web API
│   │   ├── Controllers/
│   │   ├── Models/
│   │   └── Program.cs
│   │
│   └── RestSharpClient/         # Console app using RestSharp
│       ├── Program.cs
│       └── ApiClient.cs
│
├── tests/
│   └── ProductApi.Tests/        # XUnit tests with RestSharp
│       └── ProductApiTests.cs
│
├── dotnet-restsharp-learner.sln
├── PRD.md
└── README.md
```

## 📝 Features & Endpoints

### API Endpoints (ProductApi)
| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/api/products` | Get all products |
| GET | `/api/products/{id}` | Get product by ID |
| POST | `/api/products` | Create new product |
| PUT | `/api/products/{id}` | Update existing product |
| DELETE | `/api/products/{id}` | Delete product |

### Product Model
```csharp
{
  "id": 1,
  "name": "Product Name",
  "price": 29.99,
  "description": "Product description"
}
```

## ✅ Implementation Checklist

### Phase 1: Project Setup
- [ ] Create solution file
- [ ] Create ASP.NET Core Web API project (ProductApi)
- [ ] Create Console application project (RestSharpClient)
- [ ] Create XUnit test project (ProductApi.Tests)
- [ ] Add RestSharp NuGet package to client and test projects
- [ ] Verify project structure

### Phase 2: Web API Implementation
- [ ] Create Product model class
- [ ] Create ProductsController with CRUD endpoints
- [ ] Implement in-memory data store
- [ ] Configure CORS for local development
- [ ] Add Swagger/OpenAPI for API documentation
- [ ] Test API endpoints manually

### Phase 3: RestSharp Client Implementation
- [ ] Install RestSharp in console project
- [ ] Create ApiClient class for RestSharp operations
- [ ] Implement GET all products example
- [ ] Implement GET single product example
- [ ] Implement POST (create) example
- [ ] Implement PUT (update) example
- [ ] Implement DELETE example
- [ ] Add error handling and logging
- [ ] Create menu-driven console interface

### Phase 4: Testing Implementation
- [ ] Install RestSharp in test project
- [ ] Create test class with setup/teardown
- [ ] Write test for GET all products
- [ ] Write test for GET single product
- [ ] Write test for POST (create)
- [ ] Write test for PUT (update)
- [ ] Write test for DELETE
- [ ] Write negative test cases (404, 400, etc.)

### Phase 5: Documentation & Examples
- [ ] Create comprehensive README.md
- [ ] Add code comments and XML documentation
- [ ] Include usage examples
- [ ] Add troubleshooting section
- [ ] Document RestSharp key concepts

## 🎓 Learning Outcomes
By completing this project, you will understand:
1. How to create RESTful APIs with ASP.NET Core
2. HTTP methods and status codes
3. RestSharp client configuration and usage
4. Serialization/deserialization with JSON
5. Error handling in API calls
6. Integration testing with RestSharp
7. API versioning and best practices

## 🚀 Getting Started
1. Review this PRD
2. Execute the implementation checklist in order
3. Run the API project
4. Test using the console client
5. Run automated tests
6. Experiment with modifications

## 📚 RestSharp Key Concepts to Cover
- RestClient initialization
- RestRequest configuration
- HTTP method specification
- Request headers and parameters
- Request body serialization
- Response deserialization
- Error handling (try-catch, status codes)
- Async/await patterns
- Authentication (bonus)

## 🔄 Success Criteria
- ✅ API runs without errors
- ✅ All endpoints return expected responses
- ✅ Console client successfully calls all endpoints
- ✅ All tests pass
- ✅ Code is well-documented
- ✅ README provides clear instructions

## 🎯 Future Enhancements (Optional)
- Add authentication (JWT)
- Connect to real database (SQL Server, PostgreSQL)
- Add data validation
- Implement pagination
- Add caching
- API rate limiting
- Docker containerization

---

**Estimated Time:** 2-3 hours for basic implementation
**Difficulty:** Beginner to Intermediate
