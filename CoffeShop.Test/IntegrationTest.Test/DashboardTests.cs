using System.Net;
using System.Net.Http.Headers;
using System.Text.Json;
using CoffeShop.Controllers.Services;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace IntegrationTest.DashboardTests;

public class DashboardTests : IClassFixture<WebApplicationFactory<Program>>
{
    
    private readonly WebApplicationFactory<Program> _factory;

    public DashboardTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }

    [Fact]
    [Trait("TestCase", "TQC-21")]
    [Trait("UserStory", "US4.1")]
    public async Task GetAnalyticsCorrectly_AggregatesTotalSalesFromSeededDb()
    {
        //Arrange
        var client = _factory.CreateClient();

        using var scope = _factory.Services.CreateScope();
        var tokenService = scope.ServiceProvider.GetRequiredService<ITokenService>();

        string token = tokenService.Issue("manager_test", "Manager");
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        //Act
        var response = await client.GetAsync("/api/Analytics/Analytics");

        //Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var jsonString = await response.Content.ReadAsStringAsync();

        using var jsonDocument = JsonDocument.Parse(jsonString);
        var root = jsonDocument.RootElement;

        var totalRevenue = root.GetProperty("totalRevenue").GetDecimal();
        var totalOrders = root.GetProperty("totalOrders").GetInt32();

        totalRevenue.Should().BeGreaterThan(0);
        totalOrders.Should().BeGreaterThan(0);
                
    }

}