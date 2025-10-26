using Ecommerce.Data;
using Ecommerce.Interfaces;
using Ecommerce.Models;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Services;

public class OrderService : IOrderService
{
    private readonly ApplicationDbContext _context;
    private readonly IProductService _productService;

    public OrderService(ApplicationDbContext context, IProductService productService)
    {
        _context = context;
        _productService = productService;
    }

    public async Task<Order> CreateOrderAsync(OrderRequest request)
    {
        await _productService.CheckInventoryAndReserveAsync(request.Items);

        var productIds = request.Items.Keys;
        var products = await _context.Products.Where(p => productIds.Contains(p.Id)).ToListAsync();

        var order = new Order
        {
            UserId = request.UserId,
            OrderDate = DateTime.UtcNow,
            OrderItems = new List<OrderItem>(),
            TotalAmount = 0
        };

        foreach (var item in request.Items)
        {
            var product = products.First(p => p.Id == item.Key);
            order.OrderItems.Add(new OrderItem
            {
                ProductId = item.Key,
                Quantity = item.Value,
                Price = product.Price
            });
            order.TotalAmount += product.Price * item.Value;
        }

        _context.Orders.Add(order);
        await _context.SaveChangesAsync();

        return order;
    }

    public async Task<Order?> GetOrderByIdAsync(int id)
    {
        return await _context.Orders
            .Include(o => o.OrderItems)
            .ThenInclude(oi => oi.Product)
            .FirstOrDefaultAsync(o => o.Id == id);
    }

    public async Task<bool> ConfirmOrderPaymentAsync(int orderId)
    {
        var order = await _context.Orders.FindAsync(orderId);
        if (order == null)
        {
            return false;
        }

        // In a real application, you would process the payment here.
        // For this sandbox, we'll just mark the order as paid.

        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<IEnumerable<Order>> GetOrdersByDateRangeAsync(DateTime startDate, DateTime endDate)
    {
        return await _context.Orders
            .Where(o => o.OrderDate >= startDate && o.OrderDate <= endDate)
            .ToListAsync();
    }
}
