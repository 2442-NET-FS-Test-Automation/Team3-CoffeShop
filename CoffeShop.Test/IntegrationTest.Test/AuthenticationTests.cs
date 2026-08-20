using Xunit;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Mvc.Testing;
using FluentAssertions;
using System.Net;
using CoffeShop.Data.Entities;
using System.Text.Json;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using CoffeShop.Controllers.Services;
using System.Net.Http.Headers;

namespace IntegrationTest.Test;

public class AuthenticationTests : IClassFixture<WebApplicationFactory<Program>>
{

    private readonly WebApplicationFactory<Program> _factory;

    public AuthenticationTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }
    

    [Fact]
    [Trait("TestCase", "TQC-15")]
    [Trait("UserStory", "US3.1")]
    public async Task AnonymusUser_RequestingApiInventory()
    {

        //Arrange
        var client = _factory.CreateClient();

        //Act
        var response = await client.GetAsync("api/inventory");

        //Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    
    }

    [Fact]
    [Trait("TestCase", "TQC-16")]
    [Trait("UserStory", "US3.1")]
    public async Task BaristaRoleRequesting_ApiInventory_AndRecives403Forbidden()
    {
        
        //Arrange 
        var client  = _factory.CreateClient();

        using var scope = _factory.Services.CreateScope();
        var tokenService = scope.ServiceProvider.GetRequiredService<ITokenService>();

        string token = tokenService.Issue("barista1", "Barista");
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        //Act
        var content = new StringContent("{}", Encoding.UTF8, "application/json");
        var response = await client.PostAsync("api/inventory", content);

        //Assert

        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }

    [Fact]
    [Trait("TestCase", "TQC-17")]
    [Trait("UserStory", "US3.1")]
    public async Task LoginWithInvalidPassword_Returns401Unauthorized()
    {                   

        //Arrange
        var client = _factory.CreateClient();

        var badCredentials = new
        {
            Username = "Pablo",
            Password = "pass123!"
        };

        var jsonString = JsonSerializer.Serialize(badCredentials);
        var content = new StringContent(jsonString, Encoding.UTF8, "application/json");

        //Act
        var response = await client.PostAsync("auth/login", content);


        //Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        
    }


}