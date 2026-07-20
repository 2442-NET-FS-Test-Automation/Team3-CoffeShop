using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CoffeShop.Data.Entities;


[Table("OrderLines")]

public class OrderLine
{
    public int Id {get; set;}
    public Order? Order {get; set;}
    public int OrderId {get; set;}
    public int ProductId {get; set;}
    
    [Required]
    public int Quantity {get; set;}
}