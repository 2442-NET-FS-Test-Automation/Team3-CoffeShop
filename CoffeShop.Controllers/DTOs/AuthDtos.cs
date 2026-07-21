using System.ComponentModel.DataAnnotations;

namespace CoffeShop.Controllers.DTOs;

public record RegisterDto(

    [Required, MaxLength(67)] string Username,
    [Required, MaxLength(67)] string Password,
    [Required, MaxLength(67)] string Name,
    [Required, MaxLength(67)] string Email

);

public record LoginDto(

    [Required] string Username,
    [Required] string Password

);