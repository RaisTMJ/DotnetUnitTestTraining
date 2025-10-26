using Ecommerce.Models;

namespace Ecommerce.Interfaces;

public class InsufficientInventoryException : Exception
{
    public InsufficientInventoryException(string message) : base(message)
    {
    }
}

public interface IProductService
{
    Task<Product?> GetProductByIdAsync(int id);

    Task<IEnumerable<Product>> GetAllProductsAsync();

    Task<Product> CreateProductAsync(Product newProduct);

    Task<bool> UpdateProductAsync(int id, Product updatedProduct);

    Task<bool> CheckInventoryAndReserveAsync(Dictionary<int, int> itemsToReserve);
}
