using Microsoft.AspNetCore.Mvc;
using CoffeShop.Controllers.Services;
using CoffeShop.Controllers.DTOs;
using Microsoft.AspNetCore.Authorization;

[ApiController]
[Route("api/[controller]")]
public class ReportsController : ControllerBase
{
    private readonly IReportsService _reports;

    public ReportsController(IReportsService reports)
    {
        _reports = reports;
    }

    [HttpGet("low-stock")]
    [Authorize(Roles = "Manager,Barista")]
    public async Task<ActionResult<IReadOnlyList<LowStockReportItemDto>>> LowStock(int threshold = 5)
    {
        try
        {
            var report = await _reports.GetLowStockReport(threshold);
            return Ok(report);
        } catch (ArgumentException ex)
        {
            return BadRequest( new { error = ex.Message });
        }
    }
}
