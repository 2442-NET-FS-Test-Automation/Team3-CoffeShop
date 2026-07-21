using CoffeShop.Data.Entities;
using CoffeShop.Controllers.DTOs;

namespace CoffeShop.Controllers.Services;

public interface IUserService
{

    Task<string?> RegisterAsync (RegisterDto dto);
    Task<User?> ValidateAsync (string username, string password);
    
}