using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;
using FluentAssertions;
using CoffeShop.Data.Entities;
using CoffeShop.Data;
using System.Net;
using System.Net.Http.Json;
using CoffeShop.Controllers.DTOs;


namespace IntegrationTest.Test;

public class CoffeShopApiFactory : WebApplicationFactory<Program>{}

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