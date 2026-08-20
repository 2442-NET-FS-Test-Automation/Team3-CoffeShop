using CoffeShop.Controllers.Services;
using CoffeShop.Data.Entities;
using Xunit;
using Microsoft.Extensions.Configuration;
using FluentAssertions;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace UnitTest.TokenTests;

public class TokenTests
{
    
    [Fact]
    [Trait("TestCase", "TQC-14")]
    [Trait("UserStory", "US3.1")]
    public void Issue_ShouldGenerateValidJwtWithCorrectRole()
    {
        //User for this test
        var user = new User{
        Id = 1, 
        Name ="Alan", 
        Username = "Alan1", 
        Email = "Example1@gmail.com", 
        PasswordHash = "Alan123x", 
        Role = CoffeShop.Data.Enums.RoleUsers.Barista};

        //Configuration to get the key
        var settings = new Dictionary<string, string>
        {
          {"Jwt:key", "SuperSecretKeyForTesting1234567890!"},
        };

        //Arrange
        IConfiguration configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(settings)
            .Build();

        var tokenService = new TokenService(configuration);

        //Act
        var token = tokenService.Issue(user.Username, user.Role.ToString());

        //Assert
        token.Should().NotBeEmpty();

        var handler = new JwtSecurityTokenHandler();
        var jwt = handler.ReadJwtToken(token);

        jwt.Should().NotBeNull();

        var roleClaim = jwt.Claims
            .FirstOrDefault(c => c .Type == ClaimTypes.Role);
        
        roleClaim.Should().NotBeNull();
        roleClaim!.Value.Should().Be(user.Role.ToString());

    }


}


