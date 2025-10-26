using Ecommerce.Interfaces;
using Ecommerce.Models;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Controllers;

[ApiController]
[Route("[controller]")]
public class OrdersController : ControllerBase
{
    private readonly IOrderService _orderService;

    public OrdersController(IOrderService orderService)
    {
        _orderService = orderService;
    }

    [HttpPost]
    public async Task<IActionResult> Create(OrderRequest orderRequest)
    {
        var order = await _orderService.CreateOrderAsync(orderRequest);
        return Ok(order);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var order = await _orderService.GetOrderByIdAsync(id);
        if (order == null)
        {
            return NotFound();
        }
        return Ok(order);
    }

    [HttpPost("{id}/confirm")]
    public async Task<IActionResult> ConfirmOrderPayment(int id)
    {
        var result = await _orderService.ConfirmOrderPaymentAsync(id);
        if (!result)
        {
            return NotFound();
        }
        return Ok();
    }
}
