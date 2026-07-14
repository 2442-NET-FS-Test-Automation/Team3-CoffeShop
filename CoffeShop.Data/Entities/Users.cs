using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CoffeShop.Data.Entities;

[Table("Users")]
public class User
{
    public int Id {get; set;}
    public int CostumerId {get; set;} 
    [Required, MaxLength(100)]
    public string Name {get; set;} = default!;
    [Required]
    public string Username {get; set;} = default!;
    [Required, MaxLength(100)]
    public string Email {get; set;}
    [Required]
    public string Password {get; set;} 
}