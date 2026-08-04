using CoffeShop.Controllers.Services;
using CoffeShop.Data;
using CoffeShop.Data.Entities;
using Xunit;
using FluentAssertions;
using Moq;
using CoffeShop.Controllers.DTOs;

namespace UnitTest.Tests;

public class InventoryServiceTests
{
    private readonly Mock<IInventoryRepository> _repo = new();
    [Fact]
    public async Task All_WhenRepositoryReturnsItems_ReturnsSameItems()
    {
        //Arrange
        List<InventoryItem> items = new List<InventoryItem>
        {
            new InventoryItem
            { Id = 1, ProductId = 1, Stock = 7, 
                product = new Product { Id = 1, Sku = "HOT-AME-01", Name = "American", Price = 50 }}
        };
        _repo.Setup(r => r.GetAll()).ReturnsAsync(items);
        var sut = new InventoryService(_repo.Object);

        //Act
        var result = await sut.All();

        //Assert
        result.Should().BeSameAs(items);
        _repo.Verify(r => r.GetAll(), Times.Once);
    }
    [Fact]
    public async Task BySku_WhenRepositoryFindsItem_ReturnsItem()
    {
        var sku = "HOT-AME-01";
        //Arrange
        InventoryItem item = new InventoryItem
        { Id = 1, ProductId = 1, Stock = 7, 
            product = new Product { Id = 1, Sku = sku , Name = "American", Price = 50 }
        };
        _repo.Setup(r => r.GetInventoryItemBySku(sku)).ReturnsAsync(item);
        var sut = new InventoryService(_repo.Object);

        var result = await sut.BySku(sku);
        result.Should().BeSameAs(item);
        _repo.Verify(r => r.GetInventoryItemBySku(sku), Times.Once);
    }
    [Theory]
    [InlineData("HOT-AME-01", "American", 50, 7)]
    [InlineData("HOT-LAT-02", "Latte", 65, 4)]
    [InlineData("COL-AME-07", "Iced American", 60, 9)]
    public async Task Add_WhenDtoIsProvided_PassesDtoValuesToRepository(string sku, string name, decimal price, int stock)
    {
        //Arrange
        var dto = new InventoryItemOpsDto(Sku: sku, Name: name, Price:price, Stock:stock);
        InventoryItem item = new InventoryItem
        { Id = 1, ProductId = 1, Stock = stock, 
            product = new Product { Id = 1, Sku = sku , Name = name, Price = price }
        };
        _repo.Setup(r => r.AddInventoryItem(sku, name, price, stock)).ReturnsAsync(item);
        var sut = new InventoryService(_repo.Object);
        var result = await sut.Add(dto);
        result.Should().BeSameAs(item);
        _repo.Verify(r => r.AddInventoryItem(sku, name, price, stock), Times.Once);
        result.product.Name.Should().Be(name);
        result.Stock.Should().Be(stock);
    }
}