using System.ComponentModel.DataAnnotations;

namespace Ecommerce.Models;

public class OrderRequest
{
    [Required]
    public required string UserId { get; set; }

    [Required]
    [MinLength(1)]
    public required Dictionary<int, int> Items { get; set; }
}