using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CoffeShop.Data.Entities;

[Table("Products")]

public class Product
{
    public int Id {get; set;}

    [Required, MaxLength(100)]
    public string Sku {get; set;} = default!;

    [Required, MaxLength(100)]
    public string Name {get; set;} = default!;
    
    [Required]
    public decimal Price {get; set;}
    public List<InventoryItem>? inventoryItems {get; set;}
}
