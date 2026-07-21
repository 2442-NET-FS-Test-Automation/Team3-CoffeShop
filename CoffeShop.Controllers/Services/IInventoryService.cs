using CoffeShop.Data.Entities;
using CoffeShop.Controllers.DTOs;

namespace CoffeShop.Controllers.Services;

public interface IInventoryService
{
    Task<IReadOnlyList<InventoryItem>> AllAsync();
    Task<InventoryItem?> BySkuAsync(string sku);
    public Task<InventoryItem> AddAsync(InventoryItemOpsDto dto);
    public Task<InventoryItem?> ChangeAsync(InventoryItemOpsDto dto);
    public Task<bool> RemoveAsync(string sku);
}