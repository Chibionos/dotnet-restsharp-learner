using RestSharpClient;

Console.WriteLine("╔═══════════════════════════════════════════╗");
Console.WriteLine("║   RestSharp API Client Demo              ║");
Console.WriteLine("║   Learning REST API consumption           ║");
Console.WriteLine("╚═══════════════════════════════════════════╝");
Console.WriteLine();

// API base URL - update if running on different port
const string apiBaseUrl = "https://localhost:5001";

Console.WriteLine($"API Base URL: {apiBaseUrl}");
Console.WriteLine("Make sure the ProductApi is running before using this client!");
Console.WriteLine();

var client = new ApiClient(apiBaseUrl);
bool running = true;

while (running)
{
    DisplayMenu();
    var choice = Console.ReadLine()?.Trim();

    Console.WriteLine();

    switch (choice)
    {
        case "1":
            await GetAllProducts(client);
            break;
        case "2":
            await GetProductById(client);
            break;
        case "3":
            await CreateProduct(client);
            break;
        case "4":
            await UpdateProduct(client);
            break;
        case "5":
            await DeleteProduct(client);
            break;
        case "6":
            await RunAllOperations(client);
            break;
        case "0":
            running = false;
            Console.WriteLine("Goodbye! 👋");
            break;
        default:
            Console.WriteLine("Invalid choice. Please try again.");
            break;
    }

    if (running)
    {
        Console.WriteLine("\nPress any key to continue...");
        Console.ReadKey();
        Console.Clear();
    }
}

static void DisplayMenu()
{
    Console.WriteLine("═══════════════════════════════════════════");
    Console.WriteLine("Choose an operation:");
    Console.WriteLine("═══════════════════════════════════════════");
    Console.WriteLine("1. Get All Products (GET)");
    Console.WriteLine("2. Get Product by ID (GET)");
    Console.WriteLine("3. Create New Product (POST)");
    Console.WriteLine("4. Update Product (PUT)");
    Console.WriteLine("5. Delete Product (DELETE)");
    Console.WriteLine("6. Run All Operations Demo");
    Console.WriteLine("0. Exit");
    Console.WriteLine("═══════════════════════════════════════════");
    Console.Write("Enter your choice: ");
}

static async Task GetAllProducts(ApiClient client)
{
    Console.WriteLine("→ Fetching all products...\n");
    var products = await client.GetAllProductsAsync();

    if (products != null && products.Count > 0)
    {
        Console.WriteLine("\n📦 Products List:");
        Console.WriteLine("─────────────────────────────────────────");
        foreach (var product in products)
        {
            Console.WriteLine(product);
            Console.WriteLine("─────────────────────────────────────────");
        }
    }
    else
    {
        Console.WriteLine("No products found or error occurred.");
    }
}

static async Task GetProductById(ApiClient client)
{
    Console.Write("Enter Product ID: ");
    if (int.TryParse(Console.ReadLine(), out int id))
    {
        Console.WriteLine($"\n→ Fetching product with ID {id}...\n");
        var product = await client.GetProductByIdAsync(id);

        if (product != null)
        {
            Console.WriteLine("\n📦 Product Details:");
            Console.WriteLine("─────────────────────────────────────────");
            Console.WriteLine(product);
            Console.WriteLine("─────────────────────────────────────────");
        }
    }
    else
    {
        Console.WriteLine("Invalid ID format.");
    }
}

static async Task CreateProduct(ApiClient client)
{
    Console.WriteLine("→ Creating a new product...\n");

    Console.Write("Enter product name: ");
    var name = Console.ReadLine() ?? "";

    Console.Write("Enter product price: ");
    if (!decimal.TryParse(Console.ReadLine(), out decimal price))
    {
        Console.WriteLine("Invalid price format.");
        return;
    }

    Console.Write("Enter product description: ");
    var description = Console.ReadLine() ?? "";

    Console.WriteLine($"\n→ Sending POST request...\n");
    var createdProduct = await client.CreateProductAsync(name, price, description);

    if (createdProduct != null)
    {
        Console.WriteLine("\n📦 Created Product:");
        Console.WriteLine("─────────────────────────────────────────");
        Console.WriteLine(createdProduct);
        Console.WriteLine("─────────────────────────────────────────");
    }
}

static async Task UpdateProduct(ApiClient client)
{
    Console.Write("Enter Product ID to update: ");
    if (!int.TryParse(Console.ReadLine(), out int id))
    {
        Console.WriteLine("Invalid ID format.");
        return;
    }

    Console.Write("Enter new product name: ");
    var name = Console.ReadLine() ?? "";

    Console.Write("Enter new product price: ");
    if (!decimal.TryParse(Console.ReadLine(), out decimal price))
    {
        Console.WriteLine("Invalid price format.");
        return;
    }

    Console.Write("Enter new product description: ");
    var description = Console.ReadLine() ?? "";

    Console.WriteLine($"\n→ Sending PUT request...\n");
    var updatedProduct = await client.UpdateProductAsync(id, name, price, description);

    if (updatedProduct != null)
    {
        Console.WriteLine("\n📦 Updated Product:");
        Console.WriteLine("─────────────────────────────────────────");
        Console.WriteLine(updatedProduct);
        Console.WriteLine("─────────────────────────────────────────");
    }
}

static async Task DeleteProduct(ApiClient client)
{
    Console.Write("Enter Product ID to delete: ");
    if (int.TryParse(Console.ReadLine(), out int id))
    {
        Console.WriteLine($"\n→ Sending DELETE request...\n");
        await client.DeleteProductAsync(id);
    }
    else
    {
        Console.WriteLine("Invalid ID format.");
    }
}

static async Task RunAllOperations(ApiClient client)
{
    Console.WriteLine("🎯 Running comprehensive demo of all operations...\n");

    // 1. Get all products
    Console.WriteLine("1️⃣  GET ALL PRODUCTS");
    Console.WriteLine("════════════════════════════════════════════");
    await GetAllProducts(client);
    await Task.Delay(1000);

    Console.WriteLine("\n");

    // 2. Get product by ID
    Console.WriteLine("2️⃣  GET PRODUCT BY ID (ID: 1)");
    Console.WriteLine("════════════════════════════════════════════");
    var product = await client.GetProductByIdAsync(1);
    await Task.Delay(1000);

    Console.WriteLine("\n");

    // 3. Create new product
    Console.WriteLine("3️⃣  CREATE NEW PRODUCT (POST)");
    Console.WriteLine("════════════════════════════════════════════");
    var newProduct = await client.CreateProductAsync(
        "Monitor",
        249.99m,
        "27-inch 4K UHD monitor with HDR support"
    );
    await Task.Delay(1000);

    Console.WriteLine("\n");

    // 4. Update product
    if (newProduct != null)
    {
        Console.WriteLine("4️⃣  UPDATE PRODUCT (PUT)");
        Console.WriteLine("════════════════════════════════════════════");
        await client.UpdateProductAsync(
            newProduct.Id,
            "Gaming Monitor",
            299.99m,
            "27-inch 4K UHD gaming monitor with 144Hz refresh rate"
        );
        await Task.Delay(1000);

        Console.WriteLine("\n");

        // 5. Delete product
        Console.WriteLine("5️⃣  DELETE PRODUCT (DELETE)");
        Console.WriteLine("════════════════════════════════════════════");
        await client.DeleteProductAsync(newProduct.Id);
        await Task.Delay(1000);
    }

    Console.WriteLine("\n");

    // 6. Final state
    Console.WriteLine("6️⃣  FINAL STATE - GET ALL PRODUCTS");
    Console.WriteLine("════════════════════════════════════════════");
    await GetAllProducts(client);

    Console.WriteLine("\n✅ Demo completed!");
}
