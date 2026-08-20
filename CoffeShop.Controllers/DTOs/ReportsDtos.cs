namespace CoffeShop.Controllers.DTOs;

public record LowStockReportItemDto(
    string Sku,
    string Name,
    int Stock,
    int Threshold,
    string Status
);