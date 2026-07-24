using System.ComponentModel.DataAnnotations.Schema;

namespace CoffeShop.Data.Entities;


[Table("Orders")]

public class Order
{
    public int Id {get; set;}
    public int UserId {get; set;}
    public User? User {get; set;} = default!;
    public DateTime OrderTime {get; set;} = DateTime.Now;
    public List<OrderLine>? orderLines {get; set;}

}