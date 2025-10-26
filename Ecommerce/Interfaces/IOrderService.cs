using Ecommerce.Models;

namespace Ecommerce.Interfaces;

public interface IOrderService
{
    Task<Order> CreateOrderAsync(OrderRequest request);

    Task<Order?> GetOrderByIdAsync(int id);

    Task<bool> ConfirmOrderPaymentAsync(int orderId);

    Task<IEnumerable<Order>> GetOrdersByDateRangeAsync(DateTime startDate, DateTime endDate);
}