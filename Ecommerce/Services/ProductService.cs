using Ecommerce.Data;
using Ecommerce.Interfaces;
using Ecommerce.Models;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Services;

public class ProductService : IProductService
{
    private readonly ApplicationDbContext _context;

    public ProductService(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Product?> GetProductByIdAsync(int id)
    {
        return await _context.Products.FindAsync(id);
    }

    public async Task<IEnumerable<Product>> GetAllProductsAsync()
    {
        return await _context.Products.ToListAsync();
    }

    public async Task<Product> CreateProductAsync(Product newProduct)
    {
        _context.Products.Add(newProduct);
        await _context.SaveChangesAsync();
        return newProduct;
    }

    public async Task<bool> UpdateProductAsync(int id, Product updatedProduct)
    {
        var product = await _context.Products.FindAsync(id);
        if (product == null)
        {
            return false;
        }

        product.Name = updatedProduct.Name;
        product.Price = updatedProduct.Price;
        product.StockQuantity = updatedProduct.StockQuantity;

        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> CheckInventoryAndReserveAsync(Dictionary<int, int> itemsToReserve)
    {
        var productIds = itemsToReserve.Keys;
        var products = await _context.Products.Where(p => productIds.Contains(p.Id)).ToListAsync();

        foreach (var item in itemsToReserve)
        {
            var product = products.FirstOrDefault(p => p.Id == item.Key);
            if (product == null || product.StockQuantity < item.Value)
            {
                throw new InsufficientInventoryException($"Insufficient stock for product {item.Key}");
            }
        }

        foreach (var item in itemsToReserve)
        {
            var product = products.First(p => p.Id == item.Key);
            product.StockQuantity -= item.Value;
        }

        await _context.SaveChangesAsync();
        return true;
    }
}
