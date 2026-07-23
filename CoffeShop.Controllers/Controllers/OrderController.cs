using Microsoft.AspNetCore.Mvc;
using CoffeShop.Controllers.DTOs;
using Microsoft.AspNetCore.Authorization;
using CoffeShop.Controllers.Services;
using System.Security.Claims;


[ApiController]
[Route("api")]
public class OrderController : ControllerBase
{
    private readonly IOrderService _order;


    public OrderController(IOrderService order)
    {
        _order = order;
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
