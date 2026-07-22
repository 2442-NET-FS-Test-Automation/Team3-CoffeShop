namespace CoffeShop.Controllers.DTOs;

public class AnalitycsDashboardDto
{
    
    public decimal RevenueTrend {get; set;}
    public decimal TotalRevenue {get; set;}

    public int TotalOrders {get; set;}
    public decimal OrdersTrend {get; set;}

    public decimal AverageTicket {get; set;}
    public decimal TicketTrend {get; set;}
    
    public List<HourlySalesDto> SalesByHour {get; set;} = new();
    public List<TopItemDto> TopItems {get; set;} = new();

}

public class HourlySalesDto
{
    public string Hour{get; set;}
    public decimal Amount {get; set;}


}

public class TopItemDto
{
    
    public int Rank {get; set;}
    public string Name {get; set;}
    public int UnitsSold {get; set;}

}
