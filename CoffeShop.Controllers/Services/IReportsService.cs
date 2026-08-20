using CoffeShop.Controllers.DTOs;

namespace CoffeShop.Controllers.Services;

public interface IReportsService
{
    Task<IReadOnlyList<LowStockReportItemDto>> GetLowStockReport(int threshold);
}