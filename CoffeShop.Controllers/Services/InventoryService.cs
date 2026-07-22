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

    public Task<IReadOnlyList<InventoryItem>> All()
    {   // That's the method for now
        return _repo.GetAll();
    }

    public Task<InventoryItem?> BySku(string sku)
    {
        return _repo.GetInventoryItemBySku(sku);
    }

    public Task<InventoryItem> Add(InventoryItemOpsDto dto)
    {

        return _repo.AddInventoryItem(dto.Sku, dto.Name, dto.Price, dto.Stock);
    }

    public Task<InventoryItem?> Change(InventoryItemOpsDto dto)
    {
        return _repo.ChangeInventoryItem(dto.Sku, dto.Name, dto.Price, dto.Stock);
    }

    public Task<bool> Remove(string sku)
    {
        return _repo.RemoveBySku(sku);
    }
}