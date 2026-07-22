using System.Data.Common;
using CoffeShop.Controllers.DTOs;
using CoffeShop.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Update.Internal;

namespace CoffeShop.Controllers.Controllers;

[ApiController]
[Route("/api/[controller]")]
[Authorize(Roles = "Manager")]
public class UserController : ControllerBase
{

    private readonly CoffeShopDbContext _db;

    public UserController (CoffeShopDbContext db)
    {
        
        _db = db;

    }
    

    [HttpGet("User-List")]
    public async Task<ActionResult<IEnumerable<UserDto>>> UserList( )
    {
        
        var users = await _db.Users.Select(u => new UserDto
        {
          Id = u.Id,
          Name = u.Name,
          Username = u.Username,
          Email = u.Email,
          Role = u.Role.ToString()
        })
        .ToListAsync();

        return Ok(users);    

    }

    [HttpDelete("Delete/{Id}")]
    public async Task<ActionResult> DeleteUser(int Id)
    {
        
        var user = await _db.Users.FindAsync(Id);

        if(user == null)
            return NotFound(new {message = "The user doesn't exist. Or it was already deleted"});

        _db.Users.Remove(user);
        await _db.SaveChangesAsync();

        return NoContent();
    }

    [HttpPatch("Edit/{Id}")]
    public async Task<ActionResult> EditUser (int Id, [FromBody] UserUpdateDto updateDto)
    {
        
        var user = await _db.Users.FindAsync(Id);

        if(user == null)
            return NotFound(new {message = "The user doesn't exist. Or it was already deleted"});

        if (!string.IsNullOrWhiteSpace(updateDto.Username) && updateDto.Username != user.Username){
            
            bool usernameExist = await _db.Users.AnyAsync(u => u.Username == updateDto.Username);
            if (usernameExist)
            {
                return BadRequest("The username already exist, try with another username");
            }
            user.Username = updateDto.Username;
        }

        if (!string.IsNullOrWhiteSpace(updateDto.Email))
            user.Username = updateDto.Username;

        if (!string.IsNullOrWhiteSpace(updateDto.Name))
            user.Username = updateDto.Username;
        
        if (!string.IsNullOrWhiteSpace(updateDto.Role))
            user.Username = updateDto.Username;
        
        await _db.SaveChangesAsync();

        return NoContent();
        
    }
}


