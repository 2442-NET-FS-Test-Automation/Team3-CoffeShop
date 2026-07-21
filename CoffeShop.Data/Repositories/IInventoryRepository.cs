using CoffeShop.Data.Entities;

namespace CoffeShop.Data;

public interface IInventoryRepository
{
    Task<IReadOnlyList<InventoryItem>> GetAllAsync();
    Task<InventoryItem?> GetInventoryItemBySkuAsync(string sku);
    Task<InventoryItem> AddInventoryItemAsync(string sku, string name, decimal price, int quantity);
    Task<InventoryItem?> ChangeInventoryItemAsync(string sku, string newName, decimal newPrice, int newQuantity);
    Task<bool> RemoveBySkuAsync(string sku);
}