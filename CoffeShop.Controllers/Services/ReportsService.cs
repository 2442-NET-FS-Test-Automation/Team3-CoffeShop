using CoffeShop.Controllers.DTOs;
using CoffeShop.Data;
using CoffeShop.Data.Entities;
using Microsoft.AspNetCore.Http.Features;

namespace CoffeShop.Controllers.Services;

public class ReportsService : IReportsService
{
    private readonly IInventoryRepository _repo;

    public ReportsService(IInventoryRepository repo)
    {
        _repo = repo;
    }

    public async Task<IReadOnlyList<LowStockReportItemDto>> GetLowStockReport(int threshold)
    {
        if (threshold < 0)
        {
            throw new ArgumentException("Invalid number");
        }
        
        var items = await _repo.GetAll();
        var lowStockItems = items.Where(item => item.Stock <= threshold);

        var reportItems = lowStockItems
            .Select(item => new LowStockReportItemDto(
                item.product!.Sku,
                item.product.Name,
                item.Stock,
                threshold,
                item.Stock == 0 ? "OutOfStock" : "Low"
            ))
            .ToList();

        return reportItems;
    }

}