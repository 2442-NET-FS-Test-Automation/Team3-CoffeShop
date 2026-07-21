using CoffeShop.Controllers.DTOs;
using CoffeShop.Data;
using CoffeShop.Data.Entities;
using CoffeShop.Data.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CoffeShop.Controllers.Services;

public class UserServices : IUserService
{
    
    private readonly CoffeShopDbContext _db;
    private readonly IPasswordHasher<User> _hasher;

    public UserServices(CoffeShopDbContext db, IPasswordHasher<User> hasher)
    {
        
        _db = db;
        _hasher = hasher;

    }

    public async Task<string?> RegisterAsync (RegisterDto dto)
    {

        //Trim the string   
        string cleanUsername = dto.Username.Trim();

        //And check if the username is already taken
        if( await _db.Users.AnyAsync(u => u.Username == cleanUsername))
        {
            return "Username is already taken";
        }

        User newUser = new User { 
            Username = cleanUsername, 
            Email = dto.Email,
            Name = dto.Name,
            Role = RoleUsers.Barista,
            
            }; 

        newUser.PasswordHash = _hasher.HashPassword(newUser, dto.Password);
        
        _db.Users.Add(newUser);
        await _db.SaveChangesAsync();
        
        return null;

    }

    public async Task<User?> ValidateAsync(string username, string password)
    {
        
        User? foundUser = await _db.Users.SingleOrDefaultAsync(u => u.Username == username);

        if(foundUser is null) return null;

        var result = _hasher.VerifyHashedPassword(foundUser, foundUser.PasswordHash, password);

        return result == PasswordVerificationResult.Failed ? null : foundUser;

    }

}