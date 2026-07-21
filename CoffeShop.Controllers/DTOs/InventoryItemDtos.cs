using System.ComponentModel.DataAnnotations;

namespace CoffeShop.Controllers.DTOs;

public record InventoryItemDto(string Sku, string Name, int Stock);

public record InventoryItemOpsDto(
    [Required, MaxLength(20)] string Sku,
    [Required, MaxLength(200)] string Name,
    [Required, Range(0.01, 100000)] decimal Price,
    [Required, Range(0, int.MaxValue)] int Stock
);