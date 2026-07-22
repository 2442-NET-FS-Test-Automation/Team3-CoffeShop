using CoffeShop.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace CoffeShop.Data;

public class InventoryRepository : IInventoryRepository
{
    private readonly CoffeShopDbContext _db;

    public InventoryRepository(CoffeShopDbContext db)
    {
        _db = db;
    }

    public async Task<IReadOnlyList<InventoryItem>> GetAllAsync()
    {

        return await _db.InventoryItems.Include(i => i.product).ToListAsync();
    }

    public async Task<InventoryItem?> GetInventoryItemBySkuAsync(string sku)
    {

        return await _db.InventoryItems.Include(i => i.product).FirstOrDefaultAsync(i => i.product!.Sku == sku);
    }

    public async Task<InventoryItem> AddInventoryItemAsync(string sku, string name, decimal price, int quantity)
    {

        InventoryItem newItem = new InventoryItem
        {
            product = new Product { Sku = sku, Name = name, Price = price },
            Stock = quantity
        };

        _db.InventoryItems.Add(newItem);
        await _db.SaveChangesAsync();

        return newItem;
    }

    public async Task<InventoryItem?> ChangeInventoryItemAsync(string sku, string newName, decimal newPrice, int newQuantity)
    {

        InventoryItem? item = await _db.InventoryItems
            .Include(i => i.product)
            .FirstOrDefaultAsync(i => i.product!.Sku == sku);

        if (item is null)
        {
            return null;
        }

        item.product!.Name = newName;
        item.product.Price = newPrice;
        item.Stock = newQuantity;

        await _db.SaveChangesAsync();

        return item;
    }

    public async Task<bool> RemoveBySkuAsync(string sku)
    {
        // First find the thing we want out of the database - grab it
        InventoryItem? itemToRemove = await _db.InventoryItems.Include(i => i.product)
                                            .FirstOrDefaultAsync(i => i.product!.Sku == sku);

        if (itemToRemove is null)
        {
            return false;
        }

        _db.Products.Remove(itemToRemove.product!);

        await _db.SaveChangesAsync();
        return true;

    }
}