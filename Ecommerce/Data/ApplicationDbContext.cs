using Ecommerce.Models;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<Product> Products { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderItem> OrderItems { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Product>().HasData(
            new Product { Id = 1, Name = "Widget A", Price = 19.99m, StockQuantity = 10 },
            new Product { Id = 2, Name = "Gadget B", Price = 49.50m, StockQuantity = 0 },
            new Product { Id = 3, Name = "Thing C", Price = 99.00m, StockQuantity = 5 },
            new Product { Id = 4, Name = "Device D", Price = 149.00m, StockQuantity = 2 },
            new Product { Id = 5, Name = "Appliance E", Price = 299.00m, StockQuantity = 100 }
        );
    }
}
