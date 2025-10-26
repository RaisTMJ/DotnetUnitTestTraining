using System.ComponentModel.DataAnnotations;

namespace Ecommerce.Models;

public class Order
{
    public int Id { get; set; }

    public DateTime OrderDate { get; set; }

    public List<OrderItem> OrderItems { get; set; } = new();

    public decimal TotalAmount { get; set; }

    [Required]
    public required string UserId { get; set; }
}