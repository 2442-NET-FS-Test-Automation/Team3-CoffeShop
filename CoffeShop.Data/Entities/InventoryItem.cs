using System.ComponentModel.DataAnnotations.Schema;

namespace CoffeShop.Data.Entities;

[Table("InventoryItems")]

public class InventoryItem
{
    public int Id {get; set;}
    public Product? product {get; set;}
    public int ProductId {get; set;}
    public int Stock {get; set;}

  
}