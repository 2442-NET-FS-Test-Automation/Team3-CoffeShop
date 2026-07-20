using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Identity.Client;
using Microsoft.IdentityModel.Tokens;

namespace CoffeShop.Controllers.Services;

public class TokenService : ITokenService
{
    
    private readonly string? _key;


    public TokenService(IConfiguration config){
        
        _key = config["Jwt:key"];
    }

    public string Issue (string user, string role)
    {
        //Hashing the password
        var creds = new SigningCredentials(
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_key)), SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken("coffe-user", "coffe-role",

            new[] {new Claim(ClaimTypes.Name, user), new Claim(ClaimTypes.Role, role)},
            expires: DateTime.UtcNow.AddHours(1), signingCredentials: creds

        );

        return new JwtSecurityTokenHandler().WriteToken(token);
        
    }

}