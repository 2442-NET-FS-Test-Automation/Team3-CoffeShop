using CoffeShop.Controllers.DTOs;

namespace CoffeShop.Controllers.Services;

public interface IOrderService
{
    Task<OrderDto> CreateOrderAsync(CreateOrderDto dto, string username);
}