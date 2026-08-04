using CoffeShop.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace CoffeShop.Data;

public class OrderRepository : IOrderRepository
{
    private readonly CoffeShopDbContext _db;

    public OrderRepository(CoffeShopDbContext db)
    {
        _db = db;
    }

    public async Task<User?> GetUserByUsername(string username)
    {
        return await _db.Users
            .Where(user => user.Username == username)
            .FirstOrDefaultAsync();
    }

    public async Task<IReadOnlyList<InventoryItem>> GetInventoryItemsByProductIds(IReadOnlyList<int> productIds)
    {
        return await _db.InventoryItems
            .Include(item => item.product)
            .Where(item => productIds.Contains(item.ProductId))
            .ToListAsync();
    }

    public async Task<Order> AddOrder(Order order)
    {
        await using var transaction = await _db.Database.BeginTransactionAsync();

        _db.Orders.Add(order);
        await _db.SaveChangesAsync();
        await transaction.CommitAsync();

        return order;
    }
}
