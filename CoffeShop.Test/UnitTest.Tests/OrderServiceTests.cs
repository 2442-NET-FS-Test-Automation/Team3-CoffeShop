using CoffeShop.Data.Entities;
using CoffeShop.Data;
using Xunit;
using FluentAssertions;
using CoffeShop.Controllers.Services;
using CoffeShop.Controllers.DTOs;
using Microsoft.Extensions.Caching.Memory;
using Moq;
using Microsoft.EntityFrameworkCore.Query;

namespace UnitTest.Tests;

public class OrderServiceTest
{
    [Fact]
    public async Task CreateOrderAsync_WhenOrderHasValidQuantities_CalculatesTotal()
    {
        //Arrange
        var dto = new CreateOrderDto(new List<CreateOrderLineDto>{
            new CreateOrderLineDto(1,2),
            new CreateOrderLineDto(2,1)
        });
        var repository = new Mock<IOrderRepository>();
        var cache = new MemoryCache(new MemoryCacheOptions());
        var service = new OrderService(repository.Object, cache);
        var user = new User
        {
            Id = 1,
            Name = "Ignacio",
            Username = "Admin",
            Email = "ignaciogmz99@gmail.com"
        }; 
        repository.Setup(r => r.GetUserByUsername("Admin")).ReturnsAsync(user);
        repository.Setup(r => r.GetInventoryItemsByProductIds(It.IsAny<IReadOnlyList<int>>()))
            .ReturnsAsync(new List<InventoryItem>
            {
                new InventoryItem
                {
                    ProductId = 1,
                    Stock = 2,
                    product = new Product
                    { Id = 1, Sku = "HOT-AME-01", Name = "AMERICAN", Price = 40,}
                },
                new InventoryItem
                {
                    ProductId = 2,
                    Stock = 2,
                    product = new Product
                    { Id = 2, Sku = "HOT-AME-02", Name =  "BLACK", Price = 50}
                }
            });
        repository.Setup(r => r.AddOrder(It.IsAny<Order>())).ReturnsAsync((Order order) => order);

        //Act
        var result = await service.CreateOrderAsync(dto, "Admin");

         
        result.Total.Should().Be(130);
        result.CashierName.Should().Be("Ignacio");
        result.Lines.Should().HaveCount(2);

        result.Lines.Should().Contain(line =>
            line.ProductId == 1 &&
            line.Quantity == 2 &&
            line.UnitPrice == 40 &&
            line.Subtotal == 80);

        result.Lines.Should().Contain(line =>
            line.ProductId == 2 &&
            line.Quantity == 1 &&
            line.UnitPrice == 50 &&
            line.Subtotal == 50);

        repository.Verify(r => r.GetUserByUsername("Admin"), Times.Once);
        repository.Verify(r => r.GetInventoryItemsByProductIds(It.IsAny<IReadOnlyList<int>>()), Times.Once);
        repository.Verify(r => r.AddOrder(It.IsAny<Order>()), Times.Once);
    }
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
    public async Task CreateOrderAsync_WhenOrderLineQuantityIsZero_ThrowsArgumentException()
    {
        // Arrange 
        var dto = new CreateOrderDto(new List<CreateOrderLineDto>
        {
            new CreateOrderLineDto(1, 0)
        });
        var repository = new Mock<IOrderRepository>();
        var cache = new MemoryCache(new MemoryCacheOptions());
        var service = new OrderService(repository.Object, cache);

        // Act
        Func<Task> act = () => service.CreateOrderAsync(dto, "Admin");

        // Assert
        await act.Should().ThrowAsync<ArgumentException>()
            .WithMessage("Product quantities must be greater than 0.");
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
