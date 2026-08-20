using Xunit;
using FluentAssertions;
using CoffeShop.Data.Entities;
using CoffeShop.Data;
using System.Net;
using System.Net.Http.Json;
using CoffeShop.Controllers.DTOs;

namespace IntegrationTest.Test;

[Collection("CoffeShop API")]
public class OrdersApiTest
{
    private readonly HttpClient _client;
    public OrdersApiTest(CoffeShopApiFactory factory)
    {
        _client = factory.CreateClient();
    }

    // TCQ-03: Validation rule proof - a negative quantity returns HTTP 400 Bad Request.
    [Fact]
    public async Task PostOrders_WhenOrderLineQuantityIsNegative_ReturnsBadRequest()
    {
        // Arrange
        var dto = new CreateOrderDto(new List<CreateOrderLineDto>
        {
            new CreateOrderLineDto(1, -1)
        });

        // Act
        var response = await _client.PostAsJsonAsync("/api/orders", dto);
        var content = await response.Content.ReadAsStringAsync();

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        content.Should().Contain("Quantity");
    }
}
