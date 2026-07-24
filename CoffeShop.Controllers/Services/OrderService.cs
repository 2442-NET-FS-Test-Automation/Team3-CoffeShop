using CoffeShop.Controllers.DTOs;
using CoffeShop.Data;
using CoffeShop.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace CoffeShop.Controllers.Services;

public class OrderService : IOrderService
{
    private readonly CoffeShopDbContext _db;

    public OrderService(CoffeShopDbContext db)
    {
        _db = db;
    }

    //   We need this format (CreateOrderDto dto) of lines and the token to process the orders and modify the database
    //   "lines": [
    //     { "productId": 1, "quantity": 2 },
    //     { "productId": 4, "quantity": 1 }
    //   ]
    // }

    public async Task<OrderDto> CreateOrderAsync(CreateOrderDto dto, string username)
    {   // Check if the Lines have something
        if (dto.Lines is null || dto.Lines.Count == 0)
        {
            throw new ArgumentException("The order must contain at least one product.");
        }
        
        var user = await _db.Users
            .Where(p => p.Username == username)
            .FirstOrDefaultAsync();
        // Check if the user is valid
        if (user == null)
        {
            throw new ArgumentException("User error not found");
        }

        var requestedLines = dto.Lines
            .GroupBy(line => line.ProductId)
            .Select(group => new CreateOrderLineDto(
                group.Key,
                group.Sum(line => line.Quantity)
            ))
            .ToList();

        var requestedProductIds = requestedLines.Select(line => line.ProductId).ToList();

        var inventoryItems = await _db.InventoryItems
            .Include(p => p.product)
            .Where(p => requestedProductIds.Contains(p.ProductId))
            .ToListAsync();
        
        var foundProductIds = inventoryItems.Select(item => item.ProductId).ToList();

        var missingProducts = requestedProductIds
            .Where(productId => !foundProductIds.Contains(productId))
            .ToList();

        // Check if the products are in the database
        if (missingProducts.Count > 0)
        {
            throw new ArgumentException($"Products not found: {string.Join(", ", missingProducts)}");
        }


        // Check if the order can be fulfilled
        foreach (var requestedLine in requestedLines)
        {
            var inventoryItem = inventoryItems.Single(item => item.ProductId == requestedLine.ProductId);

            if (inventoryItem.Stock < requestedLine.Quantity)
            {
                throw new InvalidOperationException(
                    $"Insufficient stock for product {requestedLine.ProductId}. Requested: {requestedLine.Quantity}, available: {inventoryItem.Stock}."
                );
            }
        }

        // Start Transaction all go in or we rollback  
        await using var transaction = await _db.Database.BeginTransactionAsync();

        // Create a new order in 
        var order = new Order
        {
            UserId = user.Id,
            orderLines = requestedLines
                .Select(line => new OrderLine
                {
                    ProductId = line.ProductId,
                    Quantity = line.Quantity
                })
                .ToList()
        };

        // Substract the Quantity from inventory Items
        foreach (var requestedLine in requestedLines)
        {
            var inventoryItem = inventoryItems.Single(item => item.ProductId == requestedLine.ProductId);
            inventoryItem.Stock -= requestedLine.Quantity;
        }

        _db.Orders.Add(order);
        await _db.SaveChangesAsync();
        await transaction.CommitAsync();

        var responseLines = order.orderLines!
            .Select(line =>
            {
                var product = inventoryItems.Single(item => item.ProductId == line.ProductId).product;
                var unitPrice = product?.Price ?? 0;

                return new OrderLineDto(
                    line.Id,
                    line.ProductId,
                    product?.Name,
                    line.Quantity,
                    unitPrice,
                    unitPrice * line.Quantity
                );
            })
            .ToList();

        return new OrderDto(
            order.Id,
            user.Id,
            user.Name,
            responseLines.Sum(line => line.Subtotal),
            responseLines
        );

    }

}
