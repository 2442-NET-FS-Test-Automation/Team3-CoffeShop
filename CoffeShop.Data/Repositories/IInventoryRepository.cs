using CoffeShop.Data.Entities;

namespace CoffeShop.Data;

public interface IInventoryRepository
{
    Task<IReadOnlyList<InventoryItem>> GetAll();
    Task<InventoryItem?> GetInventoryItemBySku(string sku);
    Task<InventoryItem> AddInventoryItem(string sku, string name, decimal price, int quantity);
    Task<InventoryItem?> ChangeInventoryItem(string sku, string newName, decimal newPrice, int newQuantity);
    Task<bool> RemoveBySku(string sku);
}