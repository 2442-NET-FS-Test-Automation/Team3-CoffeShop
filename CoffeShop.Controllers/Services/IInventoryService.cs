using CoffeShop.Data.Entities;
using CoffeShop.Controllers.DTOs;

namespace CoffeShop.Controllers.Services;

public interface IInventoryService
{
    Task<IReadOnlyList<InventoryItem>> All();
    Task<InventoryItem?> BySku(string sku);
    public Task<InventoryItem> Add(InventoryItemOpsDto dto);
    public Task<InventoryItem?> Change(InventoryItemOpsDto dto);
    public Task<bool> Remove(string sku);
}