using System.Security.Claims;
using CoffeShop.Controllers.DTOs;
using CoffeShop.Controllers.Services;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("auth")]
public class AuthController : ControllerBase
{

    private readonly ITokenService _tokens;
    private readonly IUserService _users;

    public AuthController (ITokenService token, IUserService users)
    {
        
        _tokens = token;
        _users = users;

    }

    [HttpPost("register")]
    public async Task<ActionResult> Register (RegisterDto dto)
    {
        
        var error = await _users.RegisterAsync(dto);

        if( error is not null)
        {
            return Conflict(new {error}); //Duplicate username error (409 Conflict)
        }

        return CreatedAtAction(nameof(Me), null); //201

    }


    [HttpPost ("Login")]
    public async Task<ActionResult> Login (LoginDto dto)
    {
        var user = await _users.ValidateAsync(dto.Username, dto.Password);

        if( user is null)
        {
            return Unauthorized(new {error = "Bad Credentials" });
        }

        return Ok(new {token = _tokens.Issue(user.Username, user.Role.ToString())});
    }

    [HttpGet("me")]
    public ActionResult Me()
    {
        
        return Ok(new
        {
            name = User.Identity?.Name,
            role = User.FindFirstValue(ClaimTypes.Role)
        });
    }

    [HttpPost("token")]
    public async Task<ActionResult> IssueToken (string username, string password)
    {
        var user = await _users.ValidateAsync(username, password);

        if(user is null)
        {
            return Unauthorized ("User or password are incorrect");
        }

        var userToken = _tokens.Issue(user.Username, user.Role.ToString());
        return Ok(userToken);
    }


}
