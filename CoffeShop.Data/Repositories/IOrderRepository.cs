using CoffeShop.Data.Entities;

namespace CoffeShop.Data;

public interface IOrderRepository
{
    Task<User?> GetUserByUsername(string username);
    Task<IReadOnlyList<InventoryItem>> GetInventoryItemsByProductIds(IReadOnlyList<int> productIds);
    Task<Order> AddOrder(Order order);
}
