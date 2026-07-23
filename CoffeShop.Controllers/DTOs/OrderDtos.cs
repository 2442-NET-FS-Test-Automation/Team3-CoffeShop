using System.ComponentModel.DataAnnotations;

namespace CoffeShop.Controllers.DTOs;

public record CreateOrderDto(
    [Required, MinLength(1)] List<CreateOrderLineDto> Lines
);

public record CreateOrderLineDto(
    [Range(1, int.MaxValue)] int ProductId,
    [Range(1, 100)] int Quantity
);

public record OrderDto(
    int OrderId,
    int UserId,
    string? CashierName,
    decimal Total,
    List<OrderLineDto> Lines
);

public record OrderLineDto(
    int OrderLineId,
    int ProductId,
    string? ProductName,
    int Quantity,
    decimal UnitPrice,
    decimal Subtotal
);
