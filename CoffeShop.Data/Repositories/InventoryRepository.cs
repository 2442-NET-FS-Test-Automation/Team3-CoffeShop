using CoffeShop.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace CoffeShop.Data;

public class InventoryRepository : IInventoryRepository
{
    private readonly IDbContextFactory<CoffeShopDbContext> _factory;

    public InventoryRepository(IDbContextFactory<CoffeShopDbContext> factory)
    {
        _factory = factory;
    }

    public async Task<IReadOnlyList<InventoryItem>> GetAllAsync()
    {
        await using var db = await _factory.CreateDbContextAsync();
        return await db.InventoryItems.Include(i => i.product).ToListAsync();
    }

    public async Task<InventoryItem?> GetInventoryItemBySkuAsync(string sku)
    {
        await using var db = await _factory.CreateDbContextAsync();
        return await db.InventoryItems.Include(i => i.product).FirstOrDefaultAsync(i => i.product!.Sku == sku);
    }

    public async Task<InventoryItem> AddInventoryItemAsync(string sku, string name, decimal price, int quantity)
    {
        await using var db = await _factory.CreateDbContextAsync();

        InventoryItem newItem = new InventoryItem
        {
            product = new Product { Sku = sku, Name = name, Price = price },
            Stock = quantity
        };

        db.InventoryItems.Add(newItem);
        await db.SaveChangesAsync();

        return newItem;
    }

    public async Task<InventoryItem?> ChangeInventoryItemAsync(string sku, string newName, decimal newPrice, int newQuantity)
    {
        await using var db = await _factory.CreateDbContextAsync();

        InventoryItem? item = await db.InventoryItems
            .Include(i => i.product)
            .FirstOrDefaultAsync(i => i.product!.Sku == sku);

        if (item is null)
        {
            return null;
        }

        item.product!.Name = newName;
        item.product.Price = newPrice;
        item.Stock = newQuantity;

        await db.SaveChangesAsync();

        return item;
    }

    public async Task<bool> RemoveBySkuAsync(string sku)
    {
        await using var db = await _factory.CreateDbContextAsync();

        // First find the thing we want out of the database - grab it
        InventoryItem? itemToRemove = await db.InventoryItems.Include(i => i.product)
                                            .FirstOrDefaultAsync(i => i.product!.Sku == sku);

        if (itemToRemove is null)
        {
            return false;
        }

        db.Products.Remove(itemToRemove.product!);

        await db.SaveChangesAsync();
        return true;

    }
}