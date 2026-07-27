using System.IO.Compression;
using CoffeShop.Controllers.DTOs;
using CoffeShop.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CoffeShop.Controllers.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Manager")]

public class AnalythicsController : ControllerBase
{

    private readonly CoffeShopDbContext _db;
    
    public AnalythicsController (CoffeShopDbContext db)
    {
        _db = db;
    } 

    [HttpGet("Analytics")]
    public async Task<ActionResult<AnalitycsDashboardDto>> GetDashboardData ()
    {
        
        //The total Orders
        var Totalorders = await _db.Orders.CountAsync();

        //The total sells
        var Totalrevenue = await _db.OrderLines.SumAsync(o => o.Quantity * o.Product.Price);

        //The average sales from the tickets
        var Averageticket = Totalorders > 0 ? Totalrevenue / Totalorders : 0;

        //Get the top 5 most selled products
        var topProducts = await _db.OrderLines
                .GroupBy(o => o.Product.Name)
                .Select(g => new TopItemDto
                {
                    Name = g.Key,
                    UnitsSold = g.Sum(o => o.Quantity)
                })
                .OrderByDescending(p => p.UnitsSold)
                .Take(5)
                .ToListAsync(); 

        var salesByHour = await _db.Orders
            .GroupBy( t => t.OrderTime.Hour)
            .Select(g => new HourlySalesDto
            {
            
                Hour = g.Key.ToString() + ":00",
                Amount = g.SelectMany(o => o.orderLines).Sum(o => o.Quantity * o.Product.Price)

            })
            .ToListAsync();

        

        var DashboradDto =  new AnalitycsDashboardDto
        {
          TotalOrders = Totalorders,
          TotalRevenue = Totalrevenue,
          AverageTicket = Averageticket,
          TopItems = topProducts,
          SalesByHour = salesByHour,
          RevenueTrend = 0,
          TicketTrend = 0,
          OrdersTrend = 0

        };

        return Ok(DashboradDto);

    }

}
