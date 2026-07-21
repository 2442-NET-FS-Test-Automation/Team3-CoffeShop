using CoffeShop.Controllers.DTOs;
using CoffeShop.Data;
using CoffeShop.Data.Entities;

namespace CoffeShop.Controllers.Services;

public class InventoryService : IInventoryService
{
    private readonly IInventoryRepository _repo;

    public InventoryService(IInventoryRepository repo)
    {
        _repo = repo;
    }

    public Task<IReadOnlyList<InventoryItem>> AllAsync()
    {   // That's the method for now
        return _repo.GetAllAsync();
    }

    public Task<InventoryItem?> BySkuAsync(string sku)
    {
        return _repo.GetInventoryItemBySkuAsync(sku);
    }

    public Task<InventoryItem> AddAsync(InventoryItemOpsDto dto)
    {

        return _repo.AddInventoryItemAsync(dto.Sku, dto.Name, dto.Price, dto.Stock);
    }

    public Task<InventoryItem?> ChangeAsync(InventoryItemOpsDto dto)
    {
        return _repo.ChangeInventoryItemAsync(dto.Sku, dto.Name, dto.Price, dto.Stock);
    }

    public Task<bool> RemoveAsync(string sku)
    {
        return _repo.RemoveBySkuAsync(sku);
    }
}