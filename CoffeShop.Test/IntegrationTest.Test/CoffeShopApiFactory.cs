using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;
using FluentAssertions;
using CoffeShop.Data.Entities;
using CoffeShop.Data;
using System.Net;
using System.Net.Http.Json;
using CoffeShop.Controllers.DTOs;
using System.Security.Claims;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;


namespace IntegrationTest.Test;

public class CoffeShopApiFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureTestServices(services =>
        {
            services.AddAuthentication(TestAuthHandler.AuthenticationScheme)
                .AddScheme<AuthenticationSchemeOptions, TestAuthHandler>(
                    TestAuthHandler.AuthenticationScheme,
                    options => { });
        });
    }
}

internal sealed class TestAuthHandler : AuthenticationHandler<AuthenticationSchemeOptions>
{
    public const string AuthenticationScheme = "Test";

    public TestAuthHandler(
        IOptionsMonitor<AuthenticationSchemeOptions> options,
        ILoggerFactory logger,
        UrlEncoder encoder)
        : base(options, logger, encoder)
    {
    }

    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        var claims = new[]
        {
            new Claim(ClaimTypes.Name, "Admin"),
            new Claim(ClaimTypes.Role, "Barista")
        };
        var identity = new ClaimsIdentity(claims, AuthenticationScheme);
        var principal = new ClaimsPrincipal(identity);
        var ticket = new AuthenticationTicket(principal, AuthenticationScheme);

        return Task.FromResult(AuthenticateResult.Success(ticket));
    }
}

[CollectionDefinition("CoffeShop API")]
public class CoffeShopApiCollection : ICollectionFixture<CoffeShopApiFactory>{}

[Collection("CoffeShop API")]
public class InventoryApiTests
{
    private readonly HttpClient _client;
    public InventoryApiTests(CoffeShopApiFactory factory)
    {
        _client = factory.CreateClient();
    }
    [Fact]
    public async Task GetInventory_ReturnsSuccessStatusCode()
    {
        //Act 
        var response = await _client.GetAsync("/api/inventory");
        var data = await response.Content.ReadFromJsonAsync<List<InventoryItemDto>>();

        //Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        data.Should().NotBeNull();
        data.Should().NotBeEmpty();
        data.Should().Contain(item => item.Sku == "HOT-AME-01");
    }
}
