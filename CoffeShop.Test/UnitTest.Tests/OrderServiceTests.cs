using CoffeShop.Data.Entities;
using CoffeShop.Data;
using Xunit;
using FluentAssertions;
using CoffeShop.Controllers.Services;
using CoffeShop.Controllers.DTOs;
using Microsoft.Extensions.Caching.Memory;
using Moq;

namespace UnitTest.Tests;

public class OrderServiceTest
{
    [Fact]
    public async Task CreateOrderAsync_WhenOrderHasNoLines_ThrowsArgumentException()
    {
        // Arrange 
        var dto = new CreateOrderDto(new List<CreateOrderLineDto>());
        var repository = new Mock<IOrderRepository>();
        var cache = new MemoryCache(new MemoryCacheOptions());
        var service = new OrderService(repository.Object, cache);

        // Act
        Func<Task> act = () => service.CreateOrderAsync(dto, "Admin");

        // Assert
        await act.Should().ThrowAsync<ArgumentException>();
        repository.Verify(r => r.GetUserByUsername(It.IsAny<string>()), Times.Never);
        repository.Verify(r => r.GetInventoryItemsByProductIds(It.IsAny<IReadOnlyList<int>>()), Times.Never);
        repository.Verify(r => r.AddOrder(It.IsAny<Order>()), Times.Never);
    }

    [Fact]
    public async Task CreateOrderAsync_WhenUserDoesNotExist_ThrowsArgumentException()
    {
        //Arrange
        var dto = new CreateOrderDto(new List<CreateOrderLineDto>
        {
            new CreateOrderLineDto(1,2)
        });
        var repository = new Mock<IOrderRepository>();
        repository.Setup(r => r.GetUserByUsername("Admin"))
            .ReturnsAsync((User?)null);

        var cache = new MemoryCache(new MemoryCacheOptions());
        var service = new OrderService(repository.Object, cache);

        // Act
        Func<Task> act = () => service.CreateOrderAsync(dto, "Admin");

        // Assert
        await act.Should().ThrowAsync<ArgumentException>();
        repository.Verify(r => r.GetUserByUsername("Admin"), Times.Once);
        repository.Verify(r => r.GetInventoryItemsByProductIds(It.IsAny<IReadOnlyList<int>>()), Times.Never);
        repository.Verify(r => r.AddOrder(It.IsAny<Order>()), Times.Never);
    }
    [Fact]
    public async Task CreateOrderAsync_WhenProductDoesNotExist_ThrowsArgumentException()
    {
        //Arrange
        var dto = new CreateOrderDto(new List<CreateOrderLineDto>{ new CreateOrderLineDto(1,2) });
        var repository = new Mock<IOrderRepository>();
        var user = new User
        {
            Id = 1,
            Name = "Ignacio",
            Username = "Admin",
            Email = "ignaciogmz99@gmail.com"
        };
        repository.Setup(r => r.GetUserByUsername("Admin"))
            .ReturnsAsync(user);
        repository
             .Setup(r => r.GetInventoryItemsByProductIds(It.IsAny<IReadOnlyList<int>>()))
             .ReturnsAsync(new List<InventoryItem>());
        var cache = new MemoryCache(new MemoryCacheOptions());
        var service = new OrderService(repository.Object, cache);

        //Act
        Func<Task> act = () => service.CreateOrderAsync(dto, "Admin");
        await act.Should().ThrowAsync<ArgumentException>();

        repository.Verify(r => r.GetUserByUsername("Admin"), Times.Once);
        repository.Verify(r => r.GetInventoryItemsByProductIds(It.IsAny<IReadOnlyList<int>>()), Times.Once);
        repository.Verify(r => r.AddOrder(It.IsAny<Order>()),Times.Never);

    }
    [Fact]
    public async Task CreateOrderAsync_WhenOrderIsValid_CreatesOrderAndReturnsOrderDto()
    {
       // Arrange
       var dto = new CreateOrderDto( new List<CreateOrderLineDto>{ new CreateOrderLineDto (1,2)});
       var repository = new Mock<IOrderRepository>();
       var user = new User
       {
           Id = 1,
           Name = "ignacio",
           Username = "Admin",
           Email = "ignaciogmz99@gmail.com"
       };
       repository.Setup(r => r.GetUserByUsername("Admin")).ReturnsAsync(user);
       repository
             .Setup(r => r.GetInventoryItemsByProductIds(It.IsAny<IReadOnlyList<int>>()))
             .ReturnsAsync(new List<InventoryItem>{ new InventoryItem
             {
                ProductId = 1,
                Id = 1,
                Stock = 10,
                product = new Product { Id = 1, Sku = "HOT-AME-01", Name = "American", Price = 50 }
             }});
        var cache = new MemoryCache(new MemoryCacheOptions());
        var service = new OrderService(repository.Object, cache);
        repository.Setup(r => r.AddOrder(It.IsAny<Order>()))
            .ReturnsAsync((Order order) => order);
        var result = await service.CreateOrderAsync(dto, "Admin");
        repository.Verify(r => r.GetUserByUsername("Admin"), Times.Once);
        repository.Verify(r => r.GetInventoryItemsByProductIds(It.IsAny<IReadOnlyList<int>>()), Times.Once);
        repository.Verify(r => r.AddOrder(It.IsAny<Order>()), Times.Once);
    }
}

