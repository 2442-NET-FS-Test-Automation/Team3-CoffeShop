using Microsoft.AspNetCore.Mvc;
using CoffeShop.Controllers.DTOs;
using Microsoft.AspNetCore.Authorization;
using CoffeShop.Controllers.Services;
using System.Security.Claims;
using Microsoft.Extensions.Caching.Memory;


[ApiController]
[Route("api")]
public class OrderController : ControllerBase
{
    private readonly IOrderService _order;
    private readonly IMemoryCache _cache;


    public OrderController(IOrderService order, IMemoryCache cache)
    {
        _order = order;
        _cache = cache;
    }

    [HttpPost("orders")]
    [Authorize(Roles = "Manager,Barista")]
    public async Task<ActionResult<OrderDto>> Order(CreateOrderDto dto)
    {
        //Obtain the User From the token
        var username = User.FindFirstValue(ClaimTypes.Name);

        if (string.IsNullOrWhiteSpace(username))
        {
            return Unauthorized();
        }

        try
        {
            var createdOrder = await _order.CreateOrderAsync(dto, username);
            _cache.Remove("inventory:all");
            return Created($"/api/orders/{createdOrder.OrderId}", createdOrder);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { error = ex.Message });
        }
    }
}
