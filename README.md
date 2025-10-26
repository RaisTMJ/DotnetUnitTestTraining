🛒 E-Commerce Unit Testing Sandbox (DotNet 8 + SQLite)

This project is a minimalist, headless (API-only) e-commerce backend designed specifically for training and practicing advanced unit testing techniques. It uses a clean architecture approach with layered services and dependency injection to facilitate mocking and focused testing.

🎯 Primary Goal: Unit Test Coverage

The primary goal of this application is not full feature parity with a real e-commerce platform, but rather to provide a test bed for scenarios, including:

Service Layer Mocking: Testing controllers by mocking the service layer interfaces (IProductService, IOrderService).

Repository Layer Mocking: Testing service classes by mocking the data access layer (e.g., an IRepository<T>).

Exception Handling Tests: Ensuring the application correctly handles and translates exceptions thrown by dependencies (e.g., InsufficientInventoryException, database errors).

Advanced Logic Testing: Implementing and testing complex transactional logic (e.g., order creation must reserve stock and process payment).

🛠️ Technology Stack

Backend Framework: .NET 8 ( standard MVC Controllers).

Database: SQLite (Used primarily for simple persistence, allowing developers to easily swap out data access for testing).

Data Access: Entity Framework Core (EF Core).

Testing Framework: (Recommended: xUnit, Moq/NSubstitute, FluentAssertions).

📂 Core Components & Structure

The application will follow a standard layered architecture:

Layer

Component

Purpose & Testing Focus

API

ProductsController, OrdersController

Testing Focus: Verify HTTP status codes, routing, and proper delegation to the Service layer (via mocked interfaces).

Services

ProductService, OrderService

Testing Focus: Verify core business logic, validation, exception handling, and correct interaction with the Repository layer.

Interfaces

IProductService, IOrderService

Testing Focus: Essential for DI and mocking in Controller tests. (Provided below)

Data

ApplicationDbContext

Testing Focus: Seed data. Use In-Memory databases or SQLite for integration tests.

Models

Product, Order, OrderItem

Core data structures.

📦 Main Components & Seed Data

The application will feature two primary domain components: Products and Orders.

1. Controllers (Minimum 2)

ProductsController: Manages product listings and details.

OrdersController: Handles order creation and retrieval.

2. Seed Data

To ensure immediate testability and predictable scenarios, the application should seed the database on startup:

Products: At least 5 sample products with varying stock levels (e.g., 2 in stock, 0 in stock, 100 in stock).

Name

Price

StockQuantity

Widget A

19.99

10

Gadget B

49.50

0 (Out of stock for testing failure cases)

Thing C

99.00

5

Test Users: A simple mechanism to represent a user (e.g., a hardcoded user ID/Guid) can be useful for linking orders.

``` cs
using System.Collections.Generic;
using System.Threading.Tasks;
using Ecommerce.Models; // Assume you have Product model here

namespace Ecommerce.Services
{
    // Interface for Product business logic. Crucial for unit test mocking.
    public interface IProductService
    {
        /// <summary>
        /// Retrieves a product by its unique identifier.
        /// </summary>
        Task<Product> GetProductByIdAsync(int id);

        /// <summary>
        /// Retrieves all available products.
        /// </summary>
        Task<IEnumerable<Product>> GetAllProductsAsync();

        /// <summary>
        /// Creates a new product entry.
        /// </summary>
        Task<Product> CreateProductAsync(Product newProduct);

        /// <summary>
        /// Updates an existing product's details.
        /// </summary>
        Task<bool> UpdateProductAsync(int id, Product updatedProduct);
        
        /// <summary>
        /// **Advanced Test Scenario Focus:** Checks inventory for multiple items and reserves them if available.
        /// This method is intended to test complex transactional logic and atomicity.
        /// </summary>
        /// <param name="itemsToReserve">A dictionary of ProductId and quantity.</param>
        /// <returns>True if reservation was successful.</returns>
        /// <exception cref="InsufficientInventoryException">Thrown if any item is out of stock.</exception>
        Task<bool> CheckInventoryAndReserveAsync(Dictionary<int, int> itemsToReserve);
    }
}

```
``` cs
using System.Collections.Generic;
using System.Threading.Tasks;
using Ecommerce.Models; // Assume you have Order and OrderRequest models here

namespace Ecommerce.Services
{
    // Interface for Order business logic. Focuses on complex transactions.
    public interface IOrderService
    {
        /// <summary>
        /// Creates a new order. This method should internally call IProductService.CheckInventoryAndReserveAsync
        /// and potentially an IPaymentService.
        /// **Advanced Test Scenario Focus:** Testing success, failure, and rollback scenarios.
        /// </summary>
        /// <param name="request">The order details.</param>
        Task<Order> CreateOrderAsync(OrderRequest request);

        /// <summary>
        /// Retrieves an order by its unique identifier.
        /// </summary>
        Task<Order> GetOrderByIdAsync(int id);

        /// <summary>
        /// Marks an order as confirmed after successful payment processing.
        /// </summary>
        /// <param name="orderId">The ID of the order to confirm.</param>
        /// <returns>True if confirmed successfully.</returns>
        Task<bool> ConfirmOrderPaymentAsync(int orderId);
        
        /// <summary>
        /// **Advanced Test Scenario Focus:** Retrieves orders placed within a specific date range,
        /// often involving complex query construction and data mapping.
        /// </summary>
        Task<IEnumerable<Order>> GetOrdersByDateRangeAsync(DateTime startDate, DateTime endDate);
    }
}

```
🔗 Required Interfaces (Provided)

The following interfaces must be implemented to ensure the Service layer can be easily mocked when testing the API layer, and vice-versa.
