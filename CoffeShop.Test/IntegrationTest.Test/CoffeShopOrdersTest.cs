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
}
